// -----------------------------------------------------------------
// File:    SocketChannel.cs
// Author:  mouguangyi
// Date:    2016.05.31
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.ByteStorage;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameBox.Service.NetworkManager
{
    class SocketChannel : NetworkChannel, ISocketChannel
    {
        public SocketChannel(IByteStorage storage, ProtocolType type)
        {
            this.storage = storage;
            this.type = type;
            this.sendCallback = new AsyncCallback(_SendCallback);
            this.receiveCallback = new AsyncCallback(_ReceiveCallback);
            this.buffer = new byte[DEFAULT_BUFFER_SIZE];
        }

        public override void Dispose()
        {
            Disconnect();

            base.Dispose();
        }

        public int BufferSize
        {
            set {
                var bufferSize = value << 10;
                if (this.buffer.Length != bufferSize) {
                    this.buffer = new byte[bufferSize];
                }
            }
        }

        public OnChannelStateChange OnChannelStateChange { get; set; }

        public void Connect(string ip, int port)
        {
            var endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Connect(endPoint);
        }

        public void Connect(IPEndPoint endPoint)
        {
            if (!_IsConnected()) {
                this.socket = new Socket(AddressFamily.InterNetwork, ProtocolType.Tcp == this.type ?  SocketType.Stream : SocketType.Dgram, this.type);
                this.socket.BeginConnect(endPoint, new AsyncCallback(_ConnectCallback), this.socket);
                _NotifyChannelStateChange(ChannelState.CONNECTING);
            } else {
                _NotifyChannelStateChange(ChannelState.CONNECTED);
            }
        }

        public void Disconnect()
        {
            this.connected = false;
            try {
                if (null != this.socket) {
                    if (this.socket.Connected) {
                        this.socket.Shutdown(SocketShutdown.Both);
                        this.socket.Close();
                    }
                }
            } catch (SocketException socketException) {
                Logger<INetworkManager>.E("[" + socketException.ErrorCode + "]" + socketException.Message);
            } catch (Exception exception) {
                Logger<INetworkManager>.X(exception);
            } finally {
                this.socket = null;
                while (this.sendQueue.Count > 0) {
                    var bytes = this.sendQueue.Dequeue();
                    bytes.Release();
                }
                while (this.receiveQueue.Count > 0) {
                    var bytes = this.receiveQueue.Dequeue();
                    bytes.Release();
                }
            }
        }

        public void Send(byte[] data, int size)
        {
            var bytes = this.storage.Alloc(size);
            bytes.WriteBytes(data, 0, size);
            bytes.Submit();
            _Send(bytes);
        }

        public void Send(IByteArray data)
        {
            data.Submit();
            data.Retain();
            _Send(data);
        }

        public bool Receive(Action<IByteArray> handler)
        {
            if (this.receiveQueue.Count > 0) {
                IByteArray bytes = null;
                lock (this.receiveQueue) {
                    bytes = this.receiveQueue.Dequeue();
                }
                if (null != bytes) {
                    if (null != handler) {
                        handler(bytes);
                    }
                    bytes.Release();
                }
            }

            return (this.receiveQueue.Count > 0);
        }

        private void _ConnectCallback(IAsyncResult result)
        {
            this.connected = true;
            var socket = result.AsyncState as Socket;
            try {
                socket.EndConnect(result);

                _BeginReceive();

                _NotifyChannelStateChange(ChannelState.CONNECTED);
            } catch (SocketException socketException) {
                Logger<INetworkManager>.E("[" + socketException.ErrorCode + "]" + socketException.Message);
                _Disconnect();
            } catch (Exception exception) {
                Logger<INetworkManager>.X(exception);
                _Disconnect();
            }
        }

        private void _Send(IByteArray bytes)
        {
            lock (this.sendQueue) {
                this.sendQueue.Enqueue(bytes);

                if (1 == this.sendQueue.Count) {
                    _BeginSend(this.sendQueue.Peek());
                }
            }
        }

        private void _BeginSend(IByteArray bytes)
        {
            try {
                Logger<INetworkManager>.L("Sending [" + (bytes.Size - bytes.Position) + "] bytes.");
                this.socket.BeginSend(bytes.Buffer, bytes.Offset + bytes.Position, bytes.Size - bytes.Position, SocketFlags.None, this.sendCallback, new StateObject(this.socket, bytes));
            } catch (SocketException socketException) {
                Logger<INetworkManager>.E("[" + socketException.ErrorCode + "]" + socketException.Message);
                _Disconnect();
            } catch (Exception exception) {
                Logger<INetworkManager>.X(exception);
                _Disconnect();
            }
        }

        private void _SendCallback(IAsyncResult result)
        {
            if (_IsConnected()) {
                try {
                    var state = result.AsyncState as StateObject;
                    var sentSize = state.Socket.EndSend(result);
                    if (sentSize < state.Bytes.Size) {
                        Logger<INetworkManager>.L("Message hasn't been sent completely, try to send left data!");
                        state.Bytes.Seek(sentSize);
                        _BeginSend(state.Bytes);
                    } else {
                        state.Bytes.Release();
                        lock (this.sendQueue) {
                            this.sendQueue.Dequeue();

                            if (this.sendQueue.Count > 0) {
                                _BeginSend(this.sendQueue.Peek());
                            }
                        }
                    }
                } catch (SocketException socketException) {
                    Logger<INetworkManager>.E("[" + socketException.ErrorCode + "]" + socketException.Message);
                    _Disconnect();
                } catch (Exception exception) {
                    Logger<INetworkManager>.X(exception);
                    _Disconnect();
                }
            }
        }

        private void _BeginReceive()
        {
            if (_IsConnected()) {
                try {
                    this.socket.BeginReceive(this.buffer, 0, this.buffer.Length, SocketFlags.None, this.receiveCallback, this.socket);
                } catch (SocketException socketException) {
                    Logger<INetworkManager>.E("[" + socketException.ErrorCode + "]" + socketException.Message);
                    _Disconnect();
                } catch (Exception exception) {
                    Logger<INetworkManager>.X(exception);
                    _Disconnect();
                }
            }
        }

        private void _ReceiveCallback(IAsyncResult result)
        {
            if (_IsConnected()) {
                try {
                    var socket = result.AsyncState as Socket;
                    var size = socket.EndReceive(result);
                    if (size > 0) {
                        Logger<INetworkManager>.L("Receiving [" + size + "] bytes.");

                        var bytes = this.storage.Alloc(size);
                        bytes.WriteBytes(this.buffer, 0, size);
                        bytes.Submit();
                        lock (this.receiveQueue) {
                            this.receiveQueue.Enqueue(bytes);
                        }
                        _BeginReceive();
                    } else {
                        _Disconnect();
                    }
                } catch (SocketException socketException) {
                    Logger<INetworkManager>.E("[" + socketException.ErrorCode + "]" + socketException.Message);
                    _Disconnect();
                } catch (Exception exception) {
                    Logger<INetworkManager>.X(exception);
                    _Disconnect();
                }
            }
        }

        private void _Disconnect()
        {
            if (this.connected) {
                Disconnect();
                _NotifyChannelStateChange(ChannelState.DISCONNECTED);
            }
        }

        private bool _IsConnected()
        {
            return (this.connected && null != this.socket && this.socket.Connected);
        }

        private void _NotifyChannelStateChange(string state)
        {
            new AsyncCallTask(() =>
            {
                OnChannelStateChange(state);
            }, false).Start();
        }

        private IByteStorage storage = null;
        private ProtocolType type = ProtocolType.Unknown;
        private bool connected = false;
        private Socket socket = null;
        private AsyncCallback sendCallback = null;
        private AsyncCallback receiveCallback = null;
        private byte[] buffer = null;
        private Queue<IByteArray> sendQueue = new Queue<IByteArray>();
        private Queue<IByteArray> receiveQueue = new Queue<IByteArray>();

        private const int DEFAULT_BUFFER_SIZE = 1024 * 256; // 256k

        private class StateObject
        {
            public StateObject(Socket socket, IByteArray bytes)
            {
                this.socket = socket;
                this.bytes = bytes;
            }

            public Socket Socket
            {
                get {
                    return this.socket;
                }
            }

            public IByteArray Bytes
            {
                get {
                    return this.bytes;
                }
            }

            private Socket socket = null;
            private IByteArray bytes = null;
        }
    }
}
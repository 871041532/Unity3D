// -----------------------------------------------------------------
// File:    GiantFreeServer.cs
// Author:  fuzhun
// Date:    2016.08.05
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.ByteStorage;
using GameBox.Service.NetworkManager;
using System;
using System.Collections.Generic;
using System.Net;

namespace GameBox.Service.GiantFreeServer
{
    enum ServerType
    {
        ServerType_None,
        ServerType_Fir,
        ServerType_GameWay,
    }

    class GiantFreeServer : IGiantFreeServer
    {
        public GiantFreeServer()
        { }

        public string Id
        {
            get {
                return "com.giant.service.giantfreeserver";
            }
        }

        public void Run(IServiceRunner runner)
        {
            new ServicesTask(new string[] {
                "com.giant.service.bytestorage",    // ByteStorage service
                "com.giant.service.networkmanager", // NetworkManager service
            })
            .Start()
            .Continue(task =>
            {
                var services = task.Result as IService[];
                this.storage = services[0] as IByteStorage;
                this.network = services[1] as INetworkManager;
                this.loginModule = new LoginModule(this);
                runner.Ready(_Terminate);

                return null;
            });
        }

        public void Pulse(float delta)
        {
            if (!this.connected) {
                return;
            }

            while (this.channel.Receive(bytes =>
            {
                _ReceiveMessage(bytes);
            }));
        }

        public void Start(string name, string password, ushort zoneID, string[] serverIP, int[] port, uint gameVersion, IGiantFreeClient client)
        {
            DESEncryptorFix.Reset();

            this.client = client;
            this.loginModule.Account = name;
            this.loginModule.EncryptPassWord(password);
            this.loginModule.ZoneID = zoneID;
            this.loginModule.FirServerIP = serverIP[0];
            this.loginModule.FirServerPort = port[0];
            this.loginModule.Login();
        }

        public void Stop()
        {
            if (null != this.channel) {
                this.channel.Disconnect();
                this.channel.Dispose();
                this.channel = null;
            }

            var node = this.receiveByteArray.First;
            while (null != node) {
                node.Value.Release();
                node = node.Next;
            }
            this.receiveByteArray.Clear();

            node = this.decodeByteArray.First;
            while (null != node) {
                node.Value.Release();
                node = node.Next;
            }
            this.decodeByteArray.Clear();

            this.receiveSize = 0;
            this.decodeSize = 0;
            this.onConnectSuccess = null;
            this.onConnectFailed = null;
            this.loginRecv = true;
            this.loginSend = true;
            this.connected = false;
        }

        public void SendMessage(byte cmd, byte param, byte[] data)
        {
            Message message = new Message(cmd, param, this.loginModule.GetIntervalMsecond());
            message.bodyData = data;
            _SendMessage(message);
        }

        internal void _RegMessageCallback(byte command, byte param, Action<byte[]> callback)
        {
            ushort sign = (ushort)(((int)command << 8) + param);
            this.messageCallbacks[sign] = callback;
        }

        internal void _UnregMessageCallback(byte command, byte param)
        {
            ushort sign = (ushort)(((int)command << 8) + param);
            if (this.messageCallbacks.ContainsKey(sign)) {
                this.messageCallbacks.Remove(sign);
            }
        }

        internal void _ConnectServer(ServerType type, string ip, int port, Action connectSuccess, Action connectFailed)
        {
            Stop();

            this.onConnectSuccess = connectSuccess;
            this.onConnectFailed = connectFailed;

            if (type == ServerType.ServerType_Fir) {
                this.loginRecv = true;
                this.loginSend = true;
            } else {
                this.loginRecv = false;
                this.loginSend = true;
            }

            _Connect(ip, port);
        }

        internal void _OnLoginGameSuccess()
        {
            this.loginSend = false;
        }

        internal void _NotifyClient(byte command, byte param, byte[] data)
        {
            if (null != this.client) {
                this.client.HandleMessage(command, param, data);
            }
        }

        private void _Terminate()
        {
            Stop();
            Logger<IGiantFreeServer>.L("GiantFreeServer terminate.");
        }

        private void _Connect(string ip, int port)
        {
            this.ipEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.channel = this.network.Create("tcp") as ISocketChannel;
            this.channel.OnChannelStateChange = _OnChannelStateChange;
            this.channel.Connect(this.ipEndPoint);
        }

        private void _SendMessage(IMessage message)
        {
            var bytes = (this.loginSend ? message.CompressAndRC5Encrypt() : message.CompressAndDESEncrypt());
            if (null != this.channel) {
                this.channel.Send(bytes, bytes.Length);
            }
        }

        private void _OnChannelStateChange(string state)
        {
            Logger<IGiantFreeServer>.L("ChannelStateChange state = " + state);

            switch (state) {
            case ChannelState.CONNECTED:
                this.connected = true;
                this.onConnectSuccess();
                break;
            case ChannelState.DISCONNECTED:
                this.connected = false;
                this.client.OnDisconnect(); // Notify client it lost the server
                break;
            }
        }

        private void _ReceiveMessage(IByteArray bytes)
        {
            bytes.Retain();
            this.receiveByteArray.AddLast(bytes);
            this.receiveSize += bytes.Size;
            if (this.receiveSize < 8) {
                return;
            }

            var decSize = this.receiveSize & (~7);
            this.receiveSize -= decSize;
            var decBytes = this.storage.Alloc(decSize);
            var node = this.receiveByteArray.First;
            while (decSize > 0 && null != node) {
                var current = node;
                node = node.Next;

                if (current.Value.Size <= decSize) {
                    decSize -= current.Value.Size;
                    decBytes.WriteByteArray(current.Value);
                } else {
                    var tmp = current.Value.ReadByteArray(decSize);
                    this.receiveByteArray.AddLast(current.Value.ReadByteArray());
                    decBytes.WriteByteArray(tmp);
                    tmp.Release();
                    decSize = 0;
                }
                this.receiveByteArray.Remove(current);
                current.Value.Release();
            }

            decBytes.Seek();
            byte[] decryptData = null;
            if (this.loginRecv)
                decryptData = RC5Encrypt.Decrypt(decBytes.ReadBytes());
            else {
                decryptData = decBytes.ReadBytes();
                DESEncryptorFix.EncDec_DES(decryptData, 0, decryptData.Length, false);
            }
            decBytes.Release();

            var array = this.storage.Alloc(decryptData.Length);
            array.WriteBytes(decryptData, 0, decryptData.Length);
            array.Seek();
            this.decodeByteArray.AddLast(array);
            this.decodeSize += array.Size;

            //解析
            node = this.decodeByteArray.First;
            while (this.decodeSize > 4 && null != node) {
                node = _GetHeaderNode(node);
                var byteArray = node.Value;

                //前两字节代表数据长度
                int bodyLength = byteArray.ReadInt16();
                if (bodyLength + 4 > this.decodeSize) {
                    byteArray.Seek(-2, SeekOrigin.CURRENT); // 回滚读取索引位置
                    break;
                }

                //第四字节代表是否压缩
                byteArray.ReadByte();
                int compressFlag = (int)byteArray.ReadByte();

                //有一个完整的数据包
                var commandData = this.storage.Alloc(bodyLength);
                while (commandData.Size != bodyLength) {
                    var current = node;
                    if ((current.Value.Size - current.Value.Position) <= (bodyLength - commandData.Size)) {
                        commandData.WriteByteArray(current.Value);
                        node = node.Next;
                    } else {
                        var tmp = current.Value.ReadByteArray(bodyLength - commandData.Size);
                        commandData.WriteByteArray(tmp);
                        tmp.Release();
                        node = this.decodeByteArray.AddAfter(current, current.Value.ReadByteArray());
                    }
                    current.Value.Release();
                    this.decodeByteArray.Remove(current);
                }

                this.decodeSize -= (bodyLength + 4);

                commandData.Seek();
                IByteArray msgData = null;
                if ((compressFlag & 0x40) != 0) {
                    msgData = this.storage.Alloc(BUFFERSIZE);   // 分配storage定义的BufferSize
                   // ZipCompress.Decompress(commandData, msgData);
                } else {
                    msgData = commandData;
                    msgData.Retain();
                }
                commandData.Release();

                byte command = msgData.ReadByte();
                byte param = msgData.ReadByte();

                Logger<IGiantFreeServer>.L("<<<<收到返回数据 size = " + msgData.Size + ", byCmd = " + command + ", byParam = " + param);

                ushort sign = (ushort)(((int)command << 8) + param);
                msgData.Seek(6);
                Action<byte[]> callback = null;
                if (this.messageCallbacks.TryGetValue(sign, out callback)) {
                    callback(msgData.ReadBytes());
                } else if (null != this.client) {
                    this.client.HandleMessage(command, param, msgData.ReadBytes());
                }
                msgData.Release();
            }
        }

        private LinkedListNode<IByteArray> _GetHeaderNode(LinkedListNode<IByteArray> node)
        {
            if (node.Value.Size - node.Value.Position >= 4) {
                return node;
            }

            // 字节流不足以存储数据头，则与后面的节点合并
            var size = 0;
            LinkedListNode<IByteArray> interruptNode = node;
            while (size < 4) {
                size += (interruptNode.Value.Size - interruptNode.Value.Position);
                interruptNode = interruptNode.Next;
            }

            var combineArray = this.storage.Alloc(size);
            while (node != interruptNode) {
                var current = node;
                node = node.Next;
                combineArray.WriteByteArray(current.Value);
                this.decodeByteArray.Remove(current);
            }
            combineArray.Seek();

            if (null == interruptNode) {
                return this.decodeByteArray.AddLast(combineArray);
            } else {
                return this.decodeByteArray.AddBefore(interruptNode, combineArray);
            }
        }

        private IGiantFreeClient client = null;
        private IByteStorage storage = null;
        private INetworkManager network = null;
        private ISocketChannel channel = null;
        private IPEndPoint ipEndPoint = null;
        private bool connected = false;
        private Action onConnectSuccess = null;
        private Action onConnectFailed = null;
        private bool loginRecv = true;
        private bool loginSend = true;
        private int receiveSize = 0;
        private LinkedList<IByteArray> receiveByteArray = new LinkedList<IByteArray>();
        private int decodeSize = 0;
        private LinkedList<IByteArray> decodeByteArray = new LinkedList<IByteArray>();
        private Dictionary<ushort, Action<byte[]>> messageCallbacks = new Dictionary<ushort, Action<byte[]>>();
        private LoginModule loginModule = null;

        private const int BUFFERSIZE = 64 * 1024;   // 64k
    }
}
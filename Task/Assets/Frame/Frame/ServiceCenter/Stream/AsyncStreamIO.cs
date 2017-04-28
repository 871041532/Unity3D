// -----------------------------------------------------------------
// File:    AsyncStreamIO.cs
// Author:  mouguangyi
// Date:    2016.12.08
// Description:
//      
// -----------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace GameBox.Framework
{
    sealed class AsyncStreamIO : C0
    {
        public AsyncStreamIO()
        {
            this.commands = new LinkedList<StreamIOCommand>();
        }

        public override void Dispose()
        {
            this.commands = null;

            base.Dispose();
        }

        public void AddStreamIOCommand(StreamIOCommand command)
        {
            lock (this.commands) {
                if (AsyncState.WAITING == this.state) {
                    this.commands.AddLast(command);
                } else {
                    this.insertNode = this.commands.AddAfter(this.insertNode, command);
                }
            }
        }

        public void StartAsync(int executeCountPerFrame, Action handler)
        {
            this.insertNode = this.commands.First;
            this.state = AsyncState.EXECUTING;
            new StreamIOAsyncTask(this, executeCountPerFrame).Start().Continue(task =>
            {
                if (null != handler) {
                    handler();
                }
                return null;
            });
        }

        private LinkedList<StreamIOCommand> commands = null;
        private LinkedListNode<StreamIOCommand> insertNode = null;
        private AsyncState state = AsyncState.WAITING;

        // -- Task
        private class StreamIOAsyncTask : AsyncTask
        {
            public StreamIOAsyncTask(AsyncStreamIO io, int executeCountPerFrame) : base(true)
            {
                this.io = io;
                this.executeCountPerFrame = executeCountPerFrame;
            }

            protected override bool IsDone()
            {
                var executeCountAlready = 0;
                while (this.io.commands.Count > 0 && (this.executeCountPerFrame <= 0 || executeCountAlready < this.executeCountPerFrame)) {
                    this.io.insertNode = this.io.commands.First;
                    var command = this.io.insertNode.Value;
                    command.Execute();
                    lock (this.io.commands) {
                        this.io.commands.RemoveFirst();
                    }
                    ++executeCountAlready;
                }

                return (0 == this.io.commands.Count);
            }

            private AsyncStreamIO io = null;
            private int executeCountPerFrame = -1;
        }

        private enum AsyncState
        {
            WAITING,
            EXECUTING,
            COMPLETE,
        }
    }

    internal class StreamIOCommand
    {
        public virtual void Execute()
        { }
    }
}
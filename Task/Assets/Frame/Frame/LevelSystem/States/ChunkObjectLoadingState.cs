// -----------------------------------------------------------------
// File:    ChunkObjectLoadingState.cs
// Author:  mouguangyi
// Date:    2016.12.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using System.Collections.Generic;

namespace GameBox.Service.LevelSystem
{
    sealed class ChunkObjectLoadingState : ChunkState
    {
        public ChunkObjectLoadingState(SceneChunk.States stateId) : base(stateId)
        { }

        public override void Enter(StateMachine stateMachine)
        {
            this.objectQueue = new Queue<int>();

            var chunk = stateMachine.Model as SceneChunk;
            for (var i = 0; i < chunk._Objects.Count; ++i) {
                var sceneObject = chunk._Objects[i];
                if (!sceneObject._Loaded) {
                    this.objectQueue.Enqueue(i);
                }
            }

            this.next = true;
        }

        public override void Execute(StateMachine stateMachine, float delta)
        {
            var chunk = stateMachine.Model as SceneChunk;
            if (0 == this.objectQueue.Count) {
                chunk._ChangeState(SceneChunk.States.LOADED); 
            } else if (this.next) {
                this.next = false;
                chunk._Objects[this.objectQueue.Dequeue()].LoadAsync(() =>
                {
                    this.next = true;
                });
            }
        }

        private Queue<int> objectQueue = null;
        private bool next = false;
    }
}
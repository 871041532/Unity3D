// -----------------------------------------------------------------
// File:    ChunkLoadedState.cs
// Author:  mouguangyi
// Date:    2016.12.13
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.LevelSystem
{
    sealed class ChunkLoadedState : ChunkState
    {
        public ChunkLoadedState(SceneChunk.States stateId) : base(stateId)
        { }

        public override void Enter(StateMachine stateMachine)
        {
            var chunk = stateMachine.Model as SceneChunk;
            chunk._NotifyCompleted();
        }

        public override void Execute(StateMachine stateMachine, float delta)
        {
            var chunk = stateMachine.Model as SceneChunk;
            for (var i = 0; i < chunk._Objects.Count; ++i) {
                chunk._Objects[i].OnUpdate(delta);
            }
        }
    }
}
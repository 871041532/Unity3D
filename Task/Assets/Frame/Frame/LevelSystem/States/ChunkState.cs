// -----------------------------------------------------------------
// File:    ChunkState.cs
// Author:  mouguangyi
// Date:    2016.12.14
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;

namespace GameBox.Service.LevelSystem
{
    abstract class ChunkState : State
    {
        public ChunkState(SceneChunk.States stateId)
        {
            this.stateId = stateId;
            this.loadStateId = this.unloadStateId = SceneChunk.States.INVALID;
        }

        public SceneChunk.States StateId
        {
            get {
                return this.stateId;
            }
        }

        public SceneChunk.States LoadStateId
        {
            get {
                return this.loadStateId;
            }
            set {
                this.loadStateId = value;
            }
        }

        public SceneChunk.States UnloadStateId
        {
            get {
                return this.unloadStateId;
            }
            set {
                this.unloadStateId = value;
            }
        }

        private SceneChunk.States stateId = SceneChunk.States.INVALID;
        private SceneChunk.States loadStateId = SceneChunk.States.INVALID;
        private SceneChunk.States unloadStateId = SceneChunk.States.INVALID;
    }
}
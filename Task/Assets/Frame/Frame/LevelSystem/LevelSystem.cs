// -----------------------------------------------------------------
// File:    LevelSystem.cs
// Author:  mouguangyi
// Date:    2016.11.30
// Description:
//      
// -----------------------------------------------------------------
using GameBox.Framework;
using GameBox.Service.AssetManager;
using System;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    sealed class LevelSystem : ILevelSystem, IServiceGraph
    {
        public LevelSystem()
        { }

        public string Id
        {
            get {
                return "com.giant.service.levelsystem";
            }
        }

        public void Run(IServiceRunner runner)
        {
            this.validateData = runner.GetArgs<bool>("ValidateData");

            new ServiceTask<IAssetManager>().Start().Continue(task =>
            {
                ScenePrefab.CreateRecyclePool();

                runner.Ready(_Terminate);
                return null;
            });
        }

        public void Pulse(float delta)
        {
            if (null != this.level) {
                this.level.Update(this.character.Position, this.character.Orientation, delta);
            }
        }

        public void LoadLevel(ISceneCharacter character, string levelPath, GameObject levelRoot)
        {
            this.character = character;
            this.level = new SceneLevel(levelPath, null != levelRoot ? levelRoot : new GameObject("_Level"), this.validateData);
            this.level.Load(this.character.Position, this.character.Orientation);
        }

        public void LoadLevelAsync(ISceneCharacter character, string levelPath, LevelLoadPolicy policy, GameObject levelRoot, Action handler)
        {
            this.character = character;
            this.level = new SceneLevel(levelPath, null != levelRoot ? levelRoot : new GameObject("_Level"), this.validateData);
            this.level.LoadAsync(this.character.Position, this.character.Orientation, policy, handler);
        }

        private void _Terminate()
        {
            if (null != this.level) {
                this.level.Dispose();
                this.level = null;
            }
        }

        private bool validateData = false;
        private ISceneCharacter character = null;
        private SceneLevel level = null;

        public static bool IsInEdit()
        {
            return (Application.isEditor && !Application.isPlaying);
        }

        // -- IServiceGraph
        public void Draw()
        {
            if (null != this.level) {
                this.level.DrawGraph();
            }
        }

        public float Width
        {
            get {
                return (null != this.level ? this.level.GraphWidth : 0);
            }
        }

        public float Height
        {
            get {
                return (null != this.level ? this.level.GraphHeight : 0);
            }
        }
    }
}
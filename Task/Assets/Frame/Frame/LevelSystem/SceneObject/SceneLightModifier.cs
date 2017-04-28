// -----------------------------------------------------------------
// File:    SceneLightModifier.cs
// Author:  mouguangyi
// Date:    2016.12.12
// Description:
//      
// -----------------------------------------------------------------
using System;
using UnityEngine;

namespace GameBox.Service.LevelSystem
{
    class SceneLightModifier : SceneObject
    {
        public SceneLightModifier(SceneObjectData objectData, GameObject objectRoot) : base(objectData, objectRoot)
        {
            var modifierData = objectData as SceneLightModifierData;
            this.targetIntensity = modifierData.Intensity;
            this.targetColor = new Color(modifierData.Red, modifierData.Green, modifierData.Blue);

            if (SceneLevel.IsInEdit()) {
                var modifierObject = SceneLevel.GetOrAddComponent<LightModifierObject>(this.ObjectRoot);
                modifierObject.Intensity = this.targetIntensity;
                modifierObject.Color = this.targetColor;
            }
        }

        public override void LoadAsync(Action handler)
        {
            NotifyLoaded(handler);
        }

        public override void OnEnter()
        {
            var lights = GameObject.FindObjectsOfType<Light>();
            for (var i = 0; i < lights.Length; ++i) {
                if (LightType.Directional == lights[i].type) {
                    this.light = lights[i];
                    break;
                }
            }
        }

        public override void OnExit()
        {
            this.light = null;
        }

        public override void OnUpdate(float delta)
        {
            if (null != this.light) {
                if (this.light.intensity != this.targetIntensity) {
                    var forward = (this.targetIntensity - this.light.intensity) >= 0 ? 1 : -1;
                    this.light.intensity += (forward * INTENSITY_SPEED * delta);
                    if (Mathf.Abs(this.light.intensity - this.targetIntensity) < 0.01f) {
                        this.light.intensity = this.targetIntensity;
                    }
                }

                if (this.light.color != this.targetColor) {
                    this.light.color = Color.Lerp(this.light.color, this.targetColor, delta);
                }
            }
        }

        private Light light = null;
        private float targetIntensity = 0;
        private Color targetColor;

        private const float INTENSITY_SPEED = 1f;
    }
}
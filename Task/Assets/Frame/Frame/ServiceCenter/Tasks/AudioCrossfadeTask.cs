// -----------------------------------------------------------------
// File:    AudioCrossfadeTask.cs
// Author:  mouguangyi
// Date:    2016.09.19
// Description:
//      
// -----------------------------------------------------------------
using UnityEngine;

namespace GameBox.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class AudioCrossFadeTask : AsyncTask
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="audioSource"></param>
        /// <param name="audioClip"></param>
        /// <param name="crossTime"></param>
        public AudioCrossFadeTask(AudioSource audioSource, AudioClip audioClip, float crossTime = 0.5f) : base(false)
        {
            this.audioSource = audioSource;
            this.audioClip = audioClip;
            this.audioVolume = this.audioSource.volume;
            this.audioFadeSpeed = this.audioVolume / crossTime;
            this.crossState = (null != this.audioSource.clip ? AudioCrossState.FADEOUT : AudioCrossState.SWITCH);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            var done = false;
            switch (this.crossState) {
            case AudioCrossState.FADEOUT:
                this.audioSource.volume -= Time.deltaTime * this.audioFadeSpeed;
                if (this.audioSource.volume <= 0) {
                    this.crossState = AudioCrossState.SWITCH;
                }
                break;
            case AudioCrossState.SWITCH:
                if (null == this.audioClip) {
                    this.audioSource.volume = this.audioVolume;
                    done = true;
                } else {
                    this.audioSource.clip = this.audioClip;
                    this.audioSource.volume = 0f;
                    if (!this.audioSource.isPlaying) {
                        this.audioSource.Play();
                    }
                    this.crossState = AudioCrossState.FADEIN;
                }
                break;
            case AudioCrossState.FADEIN:
                this.audioSource.volume += Time.deltaTime * this.audioFadeSpeed;
                if (this.audioSource.volume >= this.audioVolume) {
                    this.audioSource.volume = this.audioVolume;
                    done = true;
                }
                break;
            }

            return done;
        }

        private AudioSource audioSource = null;
        private AudioClip audioClip = null;
        private AudioCrossState crossState = AudioCrossState.FADEOUT;
        private float audioVolume = 0f;
        private float audioFadeSpeed = 0f;

        private enum AudioCrossState
        {
            FADEOUT,
            SWITCH,
            FADEIN,
        }
    }
}
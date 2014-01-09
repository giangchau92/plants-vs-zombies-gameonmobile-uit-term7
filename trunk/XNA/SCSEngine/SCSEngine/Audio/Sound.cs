using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SCSEngine.Audio
{
    public class Sound
    {
        private const int MAX_SOUND = 3;

        private List<SoundEffectInstance> instances;
        private SoundEffect soundEffect;
            
        public Sound(SoundEffect soundEffect)
        {
            instances = new List<SoundEffectInstance>();

            this.soundEffect = soundEffect;

            this.IsLooped = false;
        }

        public bool IsLooped
        {
            get;
            set;
        }

        private SoundEffectInstance CreateInstance()
        {
            return soundEffect.CreateInstance();
        }

        public void Play()
        {
            this.ClearStoppedSound();

            if (this.instances.Count < MAX_SOUND)
            {
                SoundEffectInstance newInstance = soundEffect.CreateInstance();
                instances.Add(newInstance);

                newInstance.Play();
            }
        }

        public void Stop()
        {
            foreach (SoundEffectInstance sei in instances)
            {
                sei.Stop();
                sei.Dispose();
            }
            this.Dispose();
        }

        public void Dispose()
        {
            foreach (SoundEffectInstance sei in instances)
            {
                sei.Stop();
                sei.Dispose();
            }

            instances.Clear();
        }

        private void ClearStoppedSound()
        {
            List<SoundEffectInstance> copyInst = new List<SoundEffectInstance>(this.instances);
            this.instances.Clear();
            foreach (SoundEffectInstance sei in copyInst)
            {
                if (sei.State != SoundState.Playing)
                {
                    sei.Dispose();
                }
                else
                {
                    this.instances.Add(sei);
                }
            }
        }
    }
}

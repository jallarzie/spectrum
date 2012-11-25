using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Spectrum.Components
{
    public class SoundPlayer
    {
        private static SoundEffect PlayerShootsEffect;

        static SoundPlayer()
        {
            PlayerShootsEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/player_shoots");    
        }

        public static void PlayPlayerShootsSound() 
        {
            SoundEffectInstance instance = PlayerShootsEffect.CreateInstance();
            instance.Play();
        }
    }
}

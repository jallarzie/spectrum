using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Spectrum.Components
{
    public class SoundPlayer
    {
        private static SoundEffect ForceFieldDisappearEffect;
        private static SoundEffect ForceFieldHitEffect;
        private static SoundEffect GameOverEffect;
        private static SoundEffect MenuItemSelectionChangeEffect;
        private static SoundEffect MenuItemSelectionClickedEffect;
        private static SoundEffect PauseTriggeredEffect;
        private static SoundEffect PauseUntriggeredEffect;
        private static SoundEffect PlayerPowersUpEffect;
        private static SoundEffect PlayerShootsEffect;
        private static SoundEffect PlayerLooseColorEffect;
        private static SoundEffect PowerCoreHitEffect;

        private static Song MainGameSong;

        static SoundPlayer()
        {
            ForceFieldDisappearEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/force_field_disappear");
            ForceFieldHitEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/force_field_hit");
            GameOverEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/game_over");
            MenuItemSelectionChangeEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/menu_item_selection_change");
            MenuItemSelectionClickedEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/menu_item_selection_clicked");
            PauseTriggeredEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/pause_triggered");
            PauseUntriggeredEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/pause_untriggered");
            PlayerPowersUpEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/player_powers_up");
			PlayerShootsEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/player_shoots");
            PlayerLooseColorEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/player_looses_color");
            PowerCoreHitEffect = Application.Instance.Content.Load<SoundEffect>("Sounds/power_core_hit");

            MainGameSong = Application.Instance.Content.Load<Song>("Sounds/Bizarre_creation_46860_choices");
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
        }

		public static void PlayForceFieldDisappearSound()
		{
            SoundEffectInstance instance = ForceFieldDisappearEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayForceFieldHitSound()
		{
            SoundEffectInstance instance = ForceFieldHitEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayGameOverSound()
		{
            SoundEffectInstance instance = GameOverEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayMenuItemSelectionChangeSound()
		{
            SoundEffectInstance instance = MenuItemSelectionChangeEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayMenuItemSelectionClickedSound()
		{
            SoundEffectInstance instance = MenuItemSelectionClickedEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayPauseTriggeredSound()
		{
            SoundEffectInstance instance = PauseTriggeredEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayPauseUntriggeredSound()
		{
            SoundEffectInstance instance = PauseUntriggeredEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayPlayerPowersUpSound()
		{
            SoundEffectInstance instance = PlayerPowersUpEffect.CreateInstance();
			instance.Play();
		}
        
		public static void PlayPlayerShootsSound() 
        {
            SoundEffectInstance instance = PlayerShootsEffect.CreateInstance();
            instance.Play();
        }

        public static void PlayPlayerLooseColorSound()
        {
            SoundEffectInstance instance = PlayerLooseColorEffect.CreateInstance();
            instance.Play();
        }


        public static void PlayPowerCoreHitSound()
		{
            SoundEffectInstance instance = PowerCoreHitEffect.CreateInstance();
			instance.Play();
		}

        public static void PlayMainGameSong() 
        {
            MediaPlayer.Play(MainGameSong);        
        }

        public static void IncreaseMainGameSoundVolume()
        {
            MediaPlayer.Volume = 1f;
        }

        public static void ReduceMainGameSongVolume() 
        {
            MediaPlayer.Volume = 0.5f;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Spectrum.Components
{
    public enum SoundEffectType 
    {
        ForceFieldDisappear,
        ForceFieldHit,
        GameOver,
        MenuItemSelectionChange,
        MenuItemSelectionClicked,
        PauseTriggered,
        PauseUntriggered,
        PlayerPowersUp,
        PlayerShoots,
        PlayerLoseColor,
        PowerCoreHit
    }

    public class SoundPlayer
    {
        private static Dictionary<SoundEffectType, SoundEffectInstance> Effects;

        private static Song MainGameSong;

        static SoundPlayer()
        {
            Effects = new Dictionary<SoundEffectType, SoundEffectInstance>();

            LoadEffect(SoundEffectType.ForceFieldDisappear, "force_field_disappear");
            LoadEffect(SoundEffectType.ForceFieldDisappear, "force_field_disappear");
            LoadEffect(SoundEffectType.ForceFieldHit, "force_field_hit");
            LoadEffect(SoundEffectType.GameOver, "game_over");
            LoadEffect(SoundEffectType.MenuItemSelectionChange, "menu_item_selection_change");
            LoadEffect(SoundEffectType.MenuItemSelectionClicked, "menu_item_selection_clicked");
            LoadEffect(SoundEffectType.PauseTriggered, "pause_triggered");
            LoadEffect(SoundEffectType.PauseUntriggered, "pause_untriggered");
            LoadEffect(SoundEffectType.PlayerPowersUp, "player_powers_up");
			LoadEffect(SoundEffectType.PlayerShoots, "player_shoots");
            LoadEffect(SoundEffectType.PlayerLoseColor, "player_loses_color");
            LoadEffect(SoundEffectType.PowerCoreHit, "power_core_hit");

            MainGameSong = Application.Instance.Content.Load<Song>("Sounds/Bizarre_creation_46860_choices");
            MediaPlayer.Stop();
            MediaPlayer.IsRepeating = true;
        }

        private static void LoadEffect(SoundEffectType effect, String filename)
        {
            Effects[effect] = Application.Instance.Content.Load<SoundEffect>("Sounds/" + filename).CreateInstance();
        }

        public static void PlayEffect(SoundEffectType effect)
        {
            Effects[effect].Play();
        }

        public static void PlayMainGameSong() 
        {
            MediaPlayer.Play(MainGameSong);        
        }

        public static void IncreaseMainGameSongVolume()
        {
            MediaPlayer.Volume = 1f;
        }

        public static void ReduceMainGameSongVolume() 
        {
            MediaPlayer.Volume = 0.5f;
        }
    }
}

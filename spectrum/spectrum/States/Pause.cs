using Microsoft.Xna.Framework;
using Spectrum.Components;

namespace Spectrum.States
{
    public class Pause : States.Menu
    {
        public Pause(States.Game gameState)
            : base(gameState, "Game paused", true)
        {
            this.AddAction("continue", delegate() {

                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return this.ReleasePreviousState();
            });

            this.AddAction("restart", delegate() {

                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Game();
            });

            this.AddAction("exit", delegate() {

                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Exit();
            });
        }
    }
}

using Spectrum.Components;

namespace Spectrum.States
{
    public class Pause : States.Menu
    {
        public Pause(States.Game gameState)
            : base(gameState, "PAUSED", true)
        {
            this.AddAction("continue", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return this.ReleasePreviousState();
            });

            this.AddAction("restart", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Game();
            });

            this.AddAction("Back to start", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Start();
            });
        }
    }
}

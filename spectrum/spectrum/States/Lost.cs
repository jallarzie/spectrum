using Spectrum.Components;

namespace Spectrum.States
{
    public class Lost : States.Menu
    {
        public Lost(States.Game gameState)
            : base(gameState, "DEFEAT", false)
        {
            this.AddAction("restart", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Game();
            });

            this.AddAction("Back to start", delegate() {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Start();
            });
        }
    }
}

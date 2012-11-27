using Spectrum.Components;

namespace Spectrum.States
{
    public class Lost : States.Menu
    {
        public Lost(States.Game gameState)
            : base(gameState, "Defeat", false)
        {
            this.AddAction("try again", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Game();
            });

            this.AddAction("exit", delegate()
            {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Exit();
            });
        }
    }
}

using Spectrum.Components;

namespace Spectrum.States
{
    public class Won : States.Menu
    {
        public Won(States.Game gameState)
            : base(gameState, "Victory !", false)
        {
            this.AddAction("next", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Game(gameState.Score);
            });
        }
    }
}

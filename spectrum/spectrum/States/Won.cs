using Spectrum.Components;

namespace Spectrum.States
{
    public class Won : States.Menu
    {
        public Won(States.Game gameState)
            : base(gameState, "VICTORY", false)
        {
            this.AddAction("next level", delegate () {
                SoundPlayer.PlayMenuItemSelectionClickedSound();
                SoundPlayer.IncreaseMainGameSoundVolume();

                return new States.Game(gameState.Level + 1, gameState.Score);
            });
        }
    }
}

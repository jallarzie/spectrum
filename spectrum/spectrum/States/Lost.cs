using Spectrum.Components;

namespace Spectrum.States
{
    public class Lost : States.Menu
    {
        public Lost(States.Game gameState)
            : base(gameState, "DEFEAT", false)
        {
            this.AddAction("restart", delegate() {
                SoundPlayer.PlayEffect(SoundEffectType.MenuItemSelectionClicked);
                SoundPlayer.IncreaseMainGameSongVolume();

                return new States.Game(1, 0, gameState.NumberOfPlayers);
            });

            this.AddAction("back to start", delegate() {
                SoundPlayer.PlayEffect(SoundEffectType.MenuItemSelectionClicked);
                SoundPlayer.IncreaseMainGameSongVolume();

                return new States.Start();
            });
        }
    }
}

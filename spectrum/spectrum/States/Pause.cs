using Spectrum.Components;

namespace Spectrum.States
{
    public class Pause : States.Menu
    {
        public Pause(States.Game gameState)
            : base(gameState, "PAUSED", true)
        {
            this.AddAction("continue", delegate () {
                SoundPlayer.PlayEffect(SoundEffectType.PauseUntriggered);
                SoundPlayer.IncreaseMainGameSongVolume();

                return this.ReleasePreviousState();
            });

            this.AddAction("restart", delegate() {
                SoundPlayer.PlayEffect(SoundEffectType.MenuItemSelectionClicked);
                SoundPlayer.IncreaseMainGameSongVolume();

                return new States.Game(1, 0, gameState.NumberOfPlayers);
            });

            this.AddAction("main menu", delegate() {
                SoundPlayer.PlayEffect(SoundEffectType.MenuItemSelectionClicked);
                SoundPlayer.IncreaseMainGameSongVolume();

                return new States.Start();
            });
        }
    }
}

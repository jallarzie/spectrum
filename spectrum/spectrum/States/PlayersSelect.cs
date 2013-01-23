using Spectrum.Components;

namespace Spectrum.States
{
    public class PlayersSelect : States.Menu
    {
        public PlayersSelect(Library.States.State state, int nbInputs)
            : base(state, "NUMBER OF PLAYERS", true)
        {
            for (int i = 1; i <= nbInputs; ++i)
            {
                int nbPlayer = i;
                this.AddAction(nbPlayer + (nbPlayer > 1 ? " players" : " player"), delegate() {
                    SoundPlayer.PlayEffect(SoundEffectType.PauseUntriggered);
                    SoundPlayer.IncreaseMainGameSongVolume();

                    return new Game(1, 0, nbPlayer);
                });
            }

            this.AddAction("back", delegate() {
                SoundPlayer.PlayEffect(SoundEffectType.MenuItemSelectionClicked);
                SoundPlayer.IncreaseMainGameSongVolume();

                return this.ReleasePreviousState();
            });
        }
    }
}

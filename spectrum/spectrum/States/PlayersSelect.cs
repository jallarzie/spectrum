using Spectrum.Components;

namespace Spectrum.States
{
    public class PlayersSelect : States.Menu
    {
        public PlayersSelect(Game gameState, int nbInputs)
            : base(gameState, "NUMBER OF PLAYERS", true)
        {
            for (int i = 1; i <= nbInputs; ++i)
            {
                int nbPlayer = i;
                this.AddAction(nbPlayer + (nbPlayer > 1 ? " players" : " player"), delegate()
                {
                    SoundPlayer.PlayEffect(SoundEffectType.PauseUntriggered);
                    SoundPlayer.IncreaseMainGameSongVolume();

                    return new Game(1, 0, nbPlayer);
                });
            }

            this.AddAction("back", delegate() {
                SoundPlayer.PlayEffect(SoundEffectType.MenuItemSelectionClicked);
                SoundPlayer.IncreaseMainGameSongVolume();

                return new States.Start();
            });
        }
    }
}

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
                return this.ReleasePreviousState();
            });

            this.AddAction("restart", delegate() {

                SoundPlayer.PlayMenuItemSelectionClickedSound();
                return new States.Game();
            });

            this.AddAction("exit", delegate() {

                SoundPlayer.PlayMenuItemSelectionClickedSound();
                return new States.Exit();
            });
        }
    }
}

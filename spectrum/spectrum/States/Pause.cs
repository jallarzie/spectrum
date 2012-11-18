using Microsoft.Xna.Framework;

namespace Spectrum.States
{
    public class Pause : States.Menu
    {
        public Pause(States.Game gameState)
            : base(gameState, "Game paused", true)
        {
            this.AddAction("continue", delegate() {
                return this.ReleasePreviousState();
            });

            this.AddAction("restart", delegate() {
                return new States.Game();
            });

            this.AddAction("exit", delegate() {
                return new States.Exit();
            });
        }
    }
}

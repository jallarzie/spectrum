using Microsoft.Xna.Framework;
using Spectrum.Components;
using Spectrum.Library.States;
using Spectrum.Library.Paths;

namespace Spectrum.States
{
    public class Game : State
    {
        public const float SPEED_PLAYER = 500f;

        public override void Initialize()
        {
            Player = new Ship();
            Player.Path = new User(Player);

            Application.Instance.Drawables.Add(new Background());
            Application.Instance.Drawables.Add(new PowerCore());
            Application.Instance.Drawables.Add(Player);
        }

        public override void Update(GameTime gameTime)
        {
            Player.Path.Move((float) (SPEED_PLAYER * gameTime.ElapsedGameTime.TotalSeconds));
        }

        private Ship Player;
    }
}

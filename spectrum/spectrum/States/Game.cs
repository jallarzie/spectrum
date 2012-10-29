using Microsoft.Xna.Framework;
using Spectrum.Components;
using Spectrum.Library.States;
using Spectrum.Library.Paths;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Spectrum.States
{
    public class Game : State
    {
        public const float SPEED_PLAYER = 750f;
        public const float SPEED_LASER = 1500f;
        public const float FIRE_RATE = 10f; // shots/sec

        public override void Initialize()
        {
            Window = new Rectangle(0, 0, 1280, 720);
            Player = new Ship();
            Player.Path = new User(Player);
            Application.Instance.Drawables.Add(Player);

            Lasers = new List<Laser>();
            LasersToRemove = new List<Laser>();
        }

        public override void Update(GameTime gameTime)
        {
            Player.Path.Move((float) (SPEED_PLAYER * gameTime.ElapsedGameTime.TotalSeconds));

            ShootLaser(gameTime);
            foreach (Laser laser in Lasers)
            {
                laser.Path.Move((float)(SPEED_LASER * gameTime.ElapsedGameTime.TotalSeconds));
                if (!laser.IsVisible(Window))
                {
                    LasersToRemove.Add(laser);
                }
            }
            foreach (Laser laser in LasersToRemove)
            {
                Lasers.Remove(laser);
                Application.Instance.Drawables.Remove(laser);
            }
            LasersToRemove.Clear();
        }

        public void ShootLaser(GameTime gameTime)
        {
            LaserFireRateCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LaserFireRateCounter >= 1 / FIRE_RATE)
            {
                LaserFireRateCounter = 0f;
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 direction = new Vector2(mouseState.X, mouseState.Y) - Player.Position;
                    Laser laser = new Laser(Player.Tint, Player.Position, direction);
                    Lasers.Add(laser);
                    Application.Instance.Drawables.Add(laser);
                }
            }
        }

        private Rectangle Window;
        private Ship Player;
        private List<Laser> Lasers, LasersToRemove;
        private float LaserFireRateCounter;
    }
}

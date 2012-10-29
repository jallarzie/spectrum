using Microsoft.Xna.Framework;
using Spectrum.Components;
using Spectrum.Library.States;
using Spectrum.Library.Paths;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace Spectrum.States
{
    public class Game : State
    {
        public const float SPEED_PLAYER = 750f;
        public const float SPEED_LASER = 1500f;
        public const float FIRE_RATE = 10f; // shots/sec
        public const float POWERUP_SPAWN_INTERVAL = 3f; // in seconds

        public override void Initialize()
        {
            RNG = new Random();
            Viewport = Application.Instance.GraphicsDevice.Viewport;
            Player = new Ship();
            Player.Position = new Vector2(Viewport.Width / 2, Viewport.Height * 4/5);
            Player.Path = new User(Player);

            Application.Instance.Drawables.Add(new Background());
            Application.Instance.Drawables.Add(new PowerCore());
            Application.Instance.Drawables.Add(Player);

            Lasers = new List<Laser>();
            LasersToRemove = new List<Laser>();
            Powerups = new List<Powerup>();
            PowerupsToRemove = new List<Powerup>();
        }

        public override void Update(GameTime gameTime)
        {
            Player.Path.Move((float) (SPEED_PLAYER * gameTime.ElapsedGameTime.TotalSeconds));

            ShootLaser(gameTime);
            foreach (Laser laser in Lasers)
            {
                laser.Path.Move((float)(SPEED_LASER * gameTime.ElapsedGameTime.TotalSeconds));
                if (!laser.IsVisible(Viewport))
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

            // to test color combining system
            SpawnRandomPowerup(gameTime);
            foreach (Powerup powerup in Powerups)
            {
                Vector2 distance = Player.Position - powerup.Position;
                if (distance.Length() <= 50)
                {
                    Player.AbsorbTint(powerup.Tint);
                    PowerupsToRemove.Add(powerup);
                }
            }
            foreach (Powerup powerup in PowerupsToRemove)
            {
                Powerups.Remove(powerup);
                Application.Instance.Drawables.Remove(powerup);
            }
            PowerupsToRemove.Clear();
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

        // to test color combining system
        public void SpawnRandomPowerup(GameTime gameTime)
        {
            PowerupSpawnCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (PowerupSpawnCounter >= POWERUP_SPAWN_INTERVAL)
            {
                PowerupSpawnCounter = 0f;
                Color color = Color.Black;
                switch (RNG.Next(7))
                {
                    case 0: color = Color.Red; break;
                    case 1: color = Color.Green; break;
                    case 2: color = Color.Blue; break;
                    case 3: color = Color.Cyan; break;
                    case 4: color = Color.Magenta; break;
                    case 5: color = Color.Yellow; break;
                    case 6: color = Color.White; break;
                }
                Powerup powerup = new Powerup(color, new Vector2(RNG.Next(Viewport.Width), RNG.Next(Viewport.Height)));
                Powerups.Add(powerup);
                Application.Instance.Drawables.Add(powerup);
            }
        }

        private Random RNG;
        private Viewport Viewport;
        private Ship Player;
        private List<Laser> Lasers, LasersToRemove;
        private List<Powerup> Powerups, PowerupsToRemove;
        private float LaserFireRateCounter, PowerupSpawnCounter;
    }
}

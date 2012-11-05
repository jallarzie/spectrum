using Microsoft.Xna.Framework;
using Spectrum.Components;
using Spectrum.Components.EnemyTypes;
using Spectrum.Library.States;
using Spectrum.Library.Paths;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Graphics;
using Spectrum.Components.EventObservers;

namespace Spectrum.States
{
    public class Game : State, PowerCoreObserver
    {
        public const float SPEED_PLAYER = 750f;
        public const float SPEED_LASER = 1500f;
        public const float FIRE_RATE = 10f; // shots/sec
        public const float ENEMY_SPAWN_RATE = 3f; // enemies/sec
        public const float POWERUP_DROP_PERCENTAGE = 0.10f;
        public const float COLLISION_DISTANCE = 50f; // pixels
        public const float DAMAGE_FEEDBACK_TIME = 0.25f; // numbers of seconds to vibrate the controller when hurt

        public override void Initialize()
        {
            RNG = new Random();
            Viewport = Application.Instance.GraphicsDevice.Viewport;
            Player = new Ship();
            Player.Position = new Vector2(Viewport.Width / 2, Viewport.Height * 4/5);
            Player.Path = new User(Player);
            Core = new PowerCore();
            Core.Observer = this;
            feedbackTime = 0f;

            Application.Instance.Drawables.Add(new Background());
            Application.Instance.Drawables.Add(Core);
            Application.Instance.Drawables.Add(Player);

            Lasers = new List<Laser>();
            LasersToRemove = new List<Laser>();
            Enemies = new List<Enemy>();
            EnemiesToRemove = new List<Enemy>();
            Powerups = new List<Powerup>();
            PowerupsToRemove = new List<Powerup>();
        }

        public override void Update(GameTime gameTime)
        {
            HandleForceFeedback(gameTime);
            Player.Path.Move((float) (SPEED_PLAYER * gameTime.ElapsedGameTime.TotalSeconds));
            ShootLaser(gameTime);
            MoveLasers(gameTime);
            Collisions();
            SpawnRandomEnemy(gameTime);
            MoveEnemies(gameTime);
            EnemyAttacks(gameTime);
            Core.Update(gameTime);
        }

        public void OnPowerCoreHealthReachedZero()
        {
            // Switch to player Wins state.
        }

        private void ShootLaser(GameTime gameTime)
        {
            LaserFireRateCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LaserFireRateCounter >= 1 / FIRE_RATE)
            {
                LaserFireRateCounter = 0f;
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 direction = new Vector2(mouseState.X, mouseState.Y) - Player.Position;
                    Laser laser = new Laser(Player.Tint, Player.Position, direction, SPEED_LASER, LaserAlignment.Player);
                    Lasers.Add(laser);
                    Application.Instance.Drawables.Add(laser);
                }
            }
        }

        private void EnemyAttacks(GameTime gameTime)
        {
            foreach (Enemy enemy in Enemies)
            {
                Laser laser = enemy.Attack((float)gameTime.ElapsedGameTime.TotalSeconds);
                if (laser != null)
                {
                    Lasers.Add(laser);
                    Application.Instance.Drawables.Add(laser);
                }
            }
            
            LaserFireRateCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (LaserFireRateCounter >= 1 / FIRE_RATE)
            {
                LaserFireRateCounter = 0f;
                MouseState mouseState = Mouse.GetState();
                GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
                if (gamepadState.IsConnected)
                {
                    if (gamepadState.ThumbSticks.Right.X !=0 || gamepadState.ThumbSticks.Right.Y != 0)
                    {
                        Vector2 direction = new Vector2(gamepadState.ThumbSticks.Right.X, -gamepadState.ThumbSticks.Right.Y);
                        Laser laser = new Laser(Player.Tint, Player.Position, direction, SPEED_LASER, LaserAlignment.Player);
                        Lasers.Add(laser);
                        Application.Instance.Drawables.Add(laser);
                    }
                }
                else
                {
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        Vector2 direction = new Vector2(mouseState.X, mouseState.Y) - Player.Position;
                        Laser laser = new Laser(Player.Tint, Player.Position, direction, SPEED_LASER, LaserAlignment.Player);
                        Lasers.Add(laser);
                        Application.Instance.Drawables.Add(laser);
                    }
                }
                
            }
        }

        private void MoveLasers(GameTime gameTime)
        {
            foreach (Laser laser in Lasers)
            {
                laser.Path.Move((float)(laser.Speed * gameTime.ElapsedGameTime.TotalSeconds));
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
        }

        private void MoveEnemies(GameTime gameTime)
        {
            foreach (Enemy enemy in Enemies)
            {
                enemy.Path.Move((float)(enemy.Speed * gameTime.ElapsedGameTime.TotalSeconds));
            }
            foreach (Enemy enemy in EnemiesToRemove)
            {
                Enemies.Remove(enemy);
                Application.Instance.Drawables.Remove(enemy);
            }
            EnemiesToRemove.Clear();
        }

        private void Collisions()
        {
            foreach (Laser laser in Lasers)
            {
                if (laser.Alignment == LaserAlignment.Player)
                {
                    // Check Collision with Power Core
                    if (laser.GetBoundingBox().CollidesWith(Core.GetBoundingSphere()))
                    {
                        Core.DecreaseHealthBy(laser.Damage);
                    }

                    foreach (Enemy enemy in Enemies)
                    {
                        Vector2 distance = enemy.Position - laser.Position;
                        if (distance.Length() <= COLLISION_DISTANCE)
                        {
                            if (RNG.NextDouble() <= POWERUP_DROP_PERCENTAGE)
                            {
                                Powerup powerup = enemy.DropPowerup(Player.Tint, RNG);
                                if (powerup.Tint != Color.Black)
                                {
                                    Powerups.Add(powerup);
                                    Application.Instance.Drawables.Add(powerup);
                                }
                            }
                            EnemiesToRemove.Add(enemy);
                        }
                    }
                }
                else if (laser.Alignment == LaserAlignment.Enemy)
                {
                    Vector2 distance = Player.Position - laser.Position;
                    if (distance.Length() <= COLLISION_DISTANCE)
                    {
                        Player.LoseTint(laser.Tint);
                        GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
                        feedbackTime = DAMAGE_FEEDBACK_TIME;
                        LasersToRemove.Add(laser);
                    }
                }
            }
            foreach (Enemy enemy in Enemies)
            {
                Vector2 distance = enemy.Position - Player.Position;
                if (distance.Length() <= COLLISION_DISTANCE)
                {
                    Player.LoseTint(enemy.Tint);
                    GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
                    feedbackTime = DAMAGE_FEEDBACK_TIME;
                    EnemiesToRemove.Add(enemy);
                }
            }
            foreach (Powerup powerup in Powerups)
            {
                Vector2 distance = Player.Position - powerup.Position;
                if (distance.Length() <= COLLISION_DISTANCE)
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

        private void SpawnRandomEnemy(GameTime gameTime)
        {
            EnemySpawnCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (EnemySpawnCounter >= 1 / ENEMY_SPAWN_RATE)
            {
                EnemySpawnCounter = 0f;
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

                // to spawn enemies just outside the playing area
                float angle = (float)RNG.NextDouble() * MathHelper.TwoPi;
                Vector2 direction = new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
                direction *= Math.Max(Viewport.Width, Viewport.Height) * (1 + (float)RNG.NextDouble() / 4);
                Vector2 center = new Vector2(Viewport.Width / 2, Viewport.Height / 2);

                Enemy enemy;
                if (RNG.Next(2) == 0)
                    enemy = new Seeker(color, center + direction, Player);
                else
                    enemy = new Observer(color, center + direction, Player);
                Enemies.Add(enemy);
                Application.Instance.Drawables.Add(enemy);
            }
        }

        private void HandleForceFeedback(GameTime gameTime)
        {
            if (feedbackTime > 0.0f)
            {
                feedbackTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            
            if (feedbackTime < 0.0f)
            {
                GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                feedbackTime = 0.0f;
            }
            
        }

        private Random RNG;
        private Viewport Viewport;
        private Ship Player;
        private PowerCore Core;
        private List<Laser> Lasers, LasersToRemove;
        private List<Enemy> Enemies, EnemiesToRemove;
        private List<Powerup> Powerups, PowerupsToRemove;
        private float LaserFireRateCounter, EnemySpawnCounter;
        private float feedbackTime;
    }
}

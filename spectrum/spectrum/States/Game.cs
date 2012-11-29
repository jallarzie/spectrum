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
using Spectrum.Library.Graphics;

namespace Spectrum.States
{
    public class Game : State, PowerCoreObserver
    {
        public const float SPEED_PLAYER = 450f;
        public const float SPEED_LASER = 600f;
        public const float FIRE_RATE = 7f; // shots/sec
        public const float LASER_MAX_CHARGE_TIME = 2f; // sec
        public const float COLLISION_DISTANCE = 30f; // pixels
        public const float DAMAGE_FEEDBACK_TIME = 0.25f; // numbers of seconds to vibrate the controller when hurt
        public const int   PLAYER_RESPAWN_THRESHOLD = 2500; // A dead player respawns every time this amount of points is exceeded, modified by level

        public Game()
            : this(1, 0, 1)
        {
        }

        public Game(int level, int score, int nbPlayers)
        {
            NumberOfPlayers = nbPlayers;

            Level = level;
            EnemyWaveSize = 2 + level + NumberOfPlayers;
            EnemyWaveSpawnTime = 10 - level / 2f;

            RNG = new Random();
            Viewport = Application.Instance.GraphicsDevice.Viewport;

            StartPositions = new Vector2[4] { new Vector2(Viewport.Width / 2, Viewport.Height * 4 / 5), 
                                                        new Vector2(Viewport.Width / 2, Viewport.Height * 1 / 5), 
                                                        new Vector2(Viewport.Width * 1 / 5, Viewport.Height / 2), 
                                                        new Vector2(Viewport.Width * 4 / 5, Viewport.Height / 2) };

            PlayerLabels = new string[4] { "P1", "P2", "P3", "P4" };

            Players = new List<Ship>();
            PlayersToRemove = new List<Ship>();
            DeadPlayers = new List<Ship>();
            PlayersToRevive = new List<Ship>();

            for (int i = 0; i < NumberOfPlayers; ++i)
            {
                if (NumberOfPlayers > 1)
                    Players.Add(new Ship((PlayerIndex)i, PlayerLabels[i]));
                else
                    Players.Add(new Ship((PlayerIndex)i, ""));
                Players[i].Position = StartPositions[i];
                Players[i].Path = new User(Players[i], new Rectangle(0, 0, Viewport.Width, Viewport.Height));
                Players[i].HealthBar.Update(new GameTime());
            }

            Crosshair = new Crosshair();
            mBackground = new Background(2000, RNG);
            Core = new PowerCore(level, RNG);
            Core.Observer = this;
            List<Entity2D> scoreKeeperEntities = new List<Entity2D>();
            Players.ForEach(player => scoreKeeperEntities.Add(player));
            ScoreKeeper = new ScoreKeeper(level, scoreKeeperEntities, PLAYER_RESPAWN_THRESHOLD * Level);
            ScoreKeeper.OnThresholdReached += RespawnPlayer;
            Score = score;
            EnemySpawnCounter = EnemyWaveSpawnTime;

            Application.Instance.Drawables.Add(mBackground);
            Application.Instance.Drawables.Add(Core);
            foreach (Ship player in Players)
                Application.Instance.Drawables.Add(player);
            Application.Instance.Drawables.Add(Crosshair);
            Application.Instance.Drawables.Add(ScoreKeeper);

            Lasers = new List<Laser>();
            LasersToRemove = new List<Laser>();
            Enemies = new List<Enemy>();
            EnemiesToRemove = new List<Enemy>();
            Powerups = new List<Powerup>();
            PowerupsToRemove = new List<Powerup>();
            Explosions = new List<Explosion>();

            SoundPlayer.PlayMainGameSong();

            //Update(new GameTime());
        }

        public override void Destroy()
        {
            Application.Instance.Drawables.Remove(mBackground);
            Application.Instance.Drawables.Remove(ScoreKeeper);
            Application.Instance.Drawables.Remove(Core);
            foreach (Ship player in Players)
                Application.Instance.Drawables.Remove(player);
            Application.Instance.Drawables.Remove(Crosshair);

            Lasers.ForEach(delegate(Laser laser) { Application.Instance.Drawables.Remove(laser); });
            LasersToRemove.ForEach(delegate(Laser laser) { Application.Instance.Drawables.Remove(laser); });

            Enemies.ForEach(delegate(Enemy enemy) { Application.Instance.Drawables.Remove(enemy); });
            EnemiesToRemove.ForEach(delegate(Enemy enemy) { Application.Instance.Drawables.Remove(enemy); });

            Powerups.ForEach(delegate(Powerup powerup) { Application.Instance.Drawables.Remove(powerup); });
            PowerupsToRemove.ForEach(delegate(Powerup powerup) { Application.Instance.Drawables.Remove(powerup); });

            Explosions.ForEach(explosion => Application.Instance.Drawables.Remove(explosion));
            Explosions.Clear();
        }

        public override bool Transition()
        {
            if (InputController.Instance.HasCalledMenu(PlayerIndex.One))
            {
                InputController.Instance.TurnOffVibration();
                SoundPlayer.ReduceMainGameSongVolume();
                SoundPlayer.PlayEffect(SoundEffectType.PauseTriggered);

                return Application.Instance.StateMachine.SetState(new States.Pause(this));
            }

            if (Players.Count == 0)
            {
                InputController.Instance.TurnOffVibration();
                return Application.Instance.StateMachine.SetState(new States.Lost(this));
            }

            if (Core.Health <= 0)
            {
                InputController.Instance.TurnOffVibration();
                return Application.Instance.StateMachine.SetState(new States.Won(this));
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            HandleForceFeedback(gameTime);
            UpdatePlayers(gameTime);
            ShootLaser(gameTime);
            MoveLasers(gameTime);
            Collisions(gameTime);
            UpdatePowerups(gameTime);
            SpawnRandomEnemyWave(gameTime);
            MoveEnemies(gameTime);
            EnemyAttacks(gameTime);
            Core.Update(gameTime);
            Explosions.ForEach(explosion => explosion.Update(gameTime));
        }

        public void OnPowerCoreHealthReachedZero()
        {
            // Switch to player Wins state.
        }

        public void OnPowerCoreHealthReduced(int Damage)
        {
            ScoreKeeper.AddPoints(Damage * ScoreKeeper.POWERCORE_HIT_SCORE_VALUE * Level);
        }

        private void ShootLaser(GameTime gameTime)
        {
            foreach (Ship player in Players)
            {
                player.LaserFireRateCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

                Vector2 direction = InputController.Instance.GetShootingDirection(player);
                if (direction != Vector2.Zero)
                    player.PathDirection((float)Math.Atan2(direction.X, -direction.Y));

                if (player.PlayerIndex == PlayerIndex.One)
                {
                    if (InputController.Instance.GetControlType(player.PlayerIndex) == ControlType.GamePad)
                        Crosshair.Position = new Vector2(-1, -1);
                    else
                        Crosshair.Position = player.Position + direction;
                }

                if (InputController.Instance.IsCharging(player.PlayerIndex))
                    player.LaserCharge += (float)(gameTime.ElapsedGameTime.TotalSeconds / LASER_MAX_CHARGE_TIME);
                else
                    player.LaserCharge = 0;

                player.LaserCharge = MathHelper.Clamp(player.LaserCharge, 0f, 1f);

                // more laser charge -> slower fire rate
                if (direction.LengthSquared() != 0 && player.LaserFireRateCounter >= (1 + player.LaserCharge * 3) / FIRE_RATE)
                {
                    Laser laser = new Laser(player.Tint, player.LaserCharge, player.Position, direction, SPEED_LASER, LaserAlignment.Player);
                    laser.Path.Move(COLLISION_DISTANCE);
                    Lasers.Add(laser);
                    Application.Instance.Drawables.Add(laser);
                    player.LaserFireRateCounter = 0f;

                    SoundPlayer.PlayEffect(SoundEffectType.PlayerShoots);
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
                enemy.Path.Move((float)(enemy.Speed * gameTime.ElapsedGameTime.TotalSeconds * (enemy.IsSlowed ? Entity2D.SLOW_SPEED_MULTIPLIER : 1f)));
                enemy.UpdateStatusEffects(gameTime);
                enemy.HealthBar.Update(gameTime);
            }
            foreach (Enemy enemy in EnemiesToRemove)
            {
                Enemies.Remove(enemy);
                Application.Instance.Drawables.Remove(enemy);
            }
            EnemiesToRemove.Clear();
        }

        private void UpdatePowerups(GameTime gameTime)
        {
            foreach (Powerup powerup in Powerups)
            {
                powerup.UpdateLifespan(gameTime);
                if (powerup.TimeToLive <= 0)
                    PowerupsToRemove.Add(powerup);
            }
            foreach (Powerup powerup in PowerupsToRemove)
            {
                Powerups.Remove(powerup);
                Application.Instance.Drawables.Remove(powerup);
            }
            PowerupsToRemove.Clear();
        }

        private void UpdatePlayers(GameTime gameTime)
        {
            foreach (Ship player in Players)
            {
                player.UpdateRespawnInvincibility(gameTime);
                player.Path.Move((float)(SPEED_PLAYER * gameTime.ElapsedGameTime.TotalSeconds * (player.IsSlowed ? Entity2D.SLOW_SPEED_MULTIPLIER : 1f)));
                player.UpdateStatusEffects(gameTime);
                player.HealthBar.Update(gameTime);

                if (player.CurrentHealthPoints <= 0)
                {
                    PlayersToRemove.Add(player);
                    Explosion explosion = player.GetExplosion(gameTime.TotalGameTime.TotalMilliseconds);
                    Explosions.Add(explosion);
                    Application.Instance.Drawables.Add(explosion);
                }
            }
            foreach (Ship player in PlayersToRemove)
            {
                GamePad.SetVibration(player.PlayerIndex, 0.0f, 0.0f);
                Players.Remove(player);
                foreach (Enemy enemy in Enemies)
                {
                    if (enemy.Target == player && Players.Count > 0)
                        enemy.ChangeTarget(Players[RNG.Next(Players.Count)]);
                }
                DeadPlayers.Add(player);
                Application.Instance.Drawables.Remove(player);
            }
            PlayersToRemove.Clear();
            foreach (Ship player in PlayersToRevive)
            {
                Players.Add(player);
                Application.Instance.Drawables.Add(player);
            }
            PlayersToRevive.Clear();
        }

        private void Collisions(GameTime gameTime)
        {   
            Vector2 distance;

            foreach (Laser laser in Lasers)
            {
                // Check Collision with Power Core
                if (laser.BoundingArea.CollidesWith(Core.BoundingArea))
                {
                    if (laser.Alignment == LaserAlignment.Player)
                        Core.ProcessHit(laser.Tint, laser.Damage);
                    LasersToRemove.Add(laser);
                }
                if (laser.Alignment == LaserAlignment.Player)
                {
                    foreach (Enemy enemy in Enemies)
                    {
                        distance = enemy.Position - laser.Position;
                        if (distance.Length() <= COLLISION_DISTANCE)
                        {
                            enemy.ProcessHit(laser);
                            if (!enemy.IsAlive() && RNG.NextDouble() + (0.75 * laser.Charge) >= 0.75)
                            {
                                Powerup powerup = enemy.DropPowerup(laser.Tint, RNG);
                                if (powerup.Tint != Color.Black)
                                {
                                    Powerups.Add(powerup);
                                    Application.Instance.Drawables.Add(powerup);
                                }
                            }
                            LasersToRemove.Add(laser);
                        }
                    }
                    foreach (Ship player in Players)
                    {
                        if (!player.IsInvincible())
                        {
                            distance = player.Position - laser.Position;
                            if (distance.Length() <= COLLISION_DISTANCE)
                                LasersToRemove.Add(laser);
                        }
                    }
                }
                else if (laser.Alignment == LaserAlignment.Enemy)
                {
                    foreach (Ship player in Players)
                    {
                        if (!player.IsInvincible())
                        {
                            distance = player.Position - laser.Position;
                            if (distance.Length() <= COLLISION_DISTANCE)
                            {
                                player.ProcessHit(laser);
                                GamePad.SetVibration(player.PlayerIndex, 0.5f, 0.5f);
                                player.FeedbackTime = DAMAGE_FEEDBACK_TIME;
                                LasersToRemove.Add(laser);
                            }
                        }
                    }
                }
            }

            foreach (Ship player in Players)
            {
                if (Core.BoundingArea.CollidesWith(player.BoundingArea))
                {
                    //Player.LoseTint(Core.Tint);
                    //GamePad.SetVibration(player.PlayerIndex, 0.5f, 0.5f);
                    //player.FeedbackTime = DAMAGE_FEEDBACK_TIME;
                    float recoilDistance = Core.CalculateCurrentRadius() * (Core.HasForceField() ? 1.35f : 1.15f) - Vector2.Distance(player.Position, Core.Position);
                    player.Path.Recoil(Core.Position, recoilDistance);
                }
            }

            foreach (Enemy enemy in Enemies)
            {
                foreach (Ship player in Players)
                {
                    if (!player.IsInvincible())
                    {
                        distance = enemy.Position - player.Position;
                        if (distance.Length() <= COLLISION_DISTANCE)
                        {
                            Color oldTint = player.Tint;
                            player.LoseTint(enemy.Tint);
                            if (oldTint == player.Tint)
                            {
                                player.CurrentHealthPoints -= enemy.CurrentHealthPoints;
                                if (player.CurrentHealthPoints < 0) player.CurrentHealthPoints = 0;
                                enemy.CurrentHealthPoints = 0;
                            }
                            GamePad.SetVibration(player.PlayerIndex, 0.5f, 0.5f);
                            player.FeedbackTime = DAMAGE_FEEDBACK_TIME;
                        }
                    }
                }
                if (!enemy.IsAlive())
                {
                    ScoreKeeper.AddPoints(enemy.GetScoreValue() * Level);
                    EnemiesToRemove.Add(enemy);

                    Explosion explosion = enemy.GetExplosion(gameTime.TotalGameTime.TotalMilliseconds);
                    Explosions.Add(explosion);
                    Application.Instance.Drawables.Add(explosion);
                }
            }

            foreach (Powerup powerup in Powerups)
            {
                foreach (Ship player in Players)
                {
                    if (!player.IsInvincible())
                    {
                        distance = player.Position - powerup.Position;
                        if (distance.Length() <= COLLISION_DISTANCE)
                        {
                            SoundPlayer.PlayEffect(SoundEffectType.PlayerPowersUp);
                            ScoreKeeper.AddPoints(50 * Level);
                            player.AbsorbTint(powerup.Tint);
                            player.CurrentHealthPoints += 50;
                            if (player.CurrentHealthPoints > player.MaxHealthPoints)
                                player.CurrentHealthPoints = player.MaxHealthPoints;
                            PowerupsToRemove.Add(powerup);
                            break;
                        }
                    }
                }
            }
        }

        private void SpawnRandomEnemyWave(GameTime gameTime)
        {
            EnemySpawnCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (EnemySpawnCounter >= EnemyWaveSpawnTime)
            {
                EnemySpawnCounter = 0f;
                float min = Math.Max(Viewport.Width / 2, Viewport.Height / 2);
                SpawnRandomEnemies(EnemyWaveSize, min, min * 1.25f);
            }
        }

        private void SpawnRandomEnemies(int num, float minDistanceFromCenter, float maxDistanceFromCenter)
        {
            for (int i = 0; i < num; i++)
            {
                Color color = Color.Black;
                switch (RNG.Next(7))
                {
                    case 0: color = Color.Red; break;
                    case 1: color = Color.Lime; break;
                    case 2: color = Color.Blue; break;
                    case 3: color = Color.Cyan; break;
                    case 4: color = Color.Magenta; break;
                    case 5: color = Color.Yellow; break;
                    case 6: color = Color.White; break;
                }

                // to spawn enemies just outside the playing area
                float angle = (float)RNG.NextDouble() * MathHelper.TwoPi;
                Vector2 direction = new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
                direction *= minDistanceFromCenter + (float)RNG.NextDouble() * (maxDistanceFromCenter - minDistanceFromCenter);
                Vector2 center = new Vector2(Viewport.Width / 2, Viewport.Height / 2);

                Vector2 position = center + direction;

                Ship target = null;
                float leastDistance = 0.0f;

                foreach (Ship player in Players)
                {
                    if (target == null)
                    {
                        target = player;
                        leastDistance = Vector2.DistanceSquared(position, player.Position);
                    }
                    else
                    {
                        float distance = Vector2.DistanceSquared(position, player.Position);
                        if (distance < leastDistance)
                            target = player;
                    }
                }

                Enemy enemy;
                if (RNG.Next(3) > 0)
                    enemy = new Seeker(color, position, target, Enemies, Core);
                else
                    enemy = new Observer(color, position, target, Enemies, Core);
                enemy.Path.Move(10);
                Enemies.Add(enemy);
                Application.Instance.Drawables.Add(enemy);
            }
        }

        private void RespawnPlayer()
        {
            if (DeadPlayers.Count > 0)
            {
                Ship respawnedPlayer = DeadPlayers[0];
                DeadPlayers.Remove(respawnedPlayer);
                PlayersToRevive.Add(respawnedPlayer);
                respawnedPlayer.Respawn();
                respawnedPlayer.Position = StartPositions[(int)respawnedPlayer.PlayerIndex];
                respawnedPlayer.Path = new User(respawnedPlayer, new Rectangle(0, 0, Viewport.Width, Viewport.Height));
            }
        }

        private void HandleForceFeedback(GameTime gameTime)
        {
            foreach (Ship player in Players)
            {
                if (player.FeedbackTime > 0.0f)
                {
                    player.FeedbackTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (player.FeedbackTime < 0.0f)
                {
                    GamePad.SetVibration(player.PlayerIndex, 0.0f, 0.0f);
                    player.FeedbackTime = 0.0f;
                }
            }
        }

        public int Level { get; private set; }
        public int NumberOfPlayers { get; protected set; }
        private Random RNG;
        private Background mBackground;
        private Viewport Viewport;
        private List<Ship> Players, PlayersToRemove, DeadPlayers, PlayersToRevive;
        private Crosshair Crosshair;
        private PowerCore Core;
        private List<Laser> Lasers, LasersToRemove;
        private List<Enemy> Enemies, EnemiesToRemove;
        private List<Powerup> Powerups, PowerupsToRemove;
        private List<Explosion> Explosions;
        private ScoreKeeper ScoreKeeper;
        private float EnemySpawnCounter, EnemyWaveSpawnTime;
        private int EnemyWaveSize;
        private Vector2[] StartPositions;
        private string[] PlayerLabels;

        public int Score
        {
            get
            {
                return ScoreKeeper.Value;
            }

            set
            {
                ScoreKeeper.Value = value;
            }
        }
    }
}

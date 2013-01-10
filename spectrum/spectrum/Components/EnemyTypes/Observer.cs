using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;
using System.Collections.Generic;
using System;

namespace Spectrum.Components.EnemyTypes
{
    public class Observer : Enemy
    {
        public Observer(Color tint, Vector2 position, Entity2D target, List<Enemy> flockmates, PowerCore core)
            : base("observerEnemy", tint, position, target)
        {
            CurrentHealthPoints = MaxHealthPoints = 100;
            Path = new Flock(this, flockmates, core, target, 400, new Random().Next(75, 150));
            Scale = 1.1f;
            Speed = 290f;
            FireRate = 2.5f;
        }

        public override Laser Attack(float seconds)
        {
            LastFired += seconds;

            if (LastFired >= FireRate)
            {
                LastFired -= FireRate;

                Vector2 direction = Target.Position - Position;
                Laser laser = new Laser(Tint, 0, Position, direction, 300f, LaserAlignment.Enemy);
                laser.Path.Move(30);
                return laser;
            }

            return null;
        }

        public override int GetScoreValue()
        {
            return 20;
        }

        public override Explosion GetExplosion(double destroyedTime)
        {
            return new Explosion(this, destroyedTime);
        }
    }
}

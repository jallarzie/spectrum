using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;
using System.Collections.Generic;
using System;

namespace Spectrum.Components.EnemyTypes
{
    public class Seeker : Enemy
    {
        public Seeker(Color tint, Vector2 position, Entity2D target, List<Enemy> flockmates, PowerCore core)
            : base("seekerEnemy", tint, position, target)
        {
            CurrentHealthPoints = MaxHealthPoints = 30;
            Path = new Flock(this, flockmates, core, target, 0, 0);
            Scale = 0.5f;
            Speed = 250f;
            FireRate = 0f;
        }

        public override Laser Attack(float seconds)
        {
            return null;
        }

        public override int GetScoreValue()
        {
            return 10;
        }

        public override Explosion GetExplosion(double destroyedTime)
        {
            Explosion e = new Explosion(this, destroyedTime);
            e.Scale = 0.5f;
            return e;
        }
    }
}

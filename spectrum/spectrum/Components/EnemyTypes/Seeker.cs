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
        public Seeker(Color tint, Vector2 position, Entity2D target)
            : base("seekerEnemy", tint, position, target)
        {
            Path = new Follow(this, target);
            Scale = 0.2f;
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

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
            : base(tint, position, target)
        {
            Path = new Follow(this, target);
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

    }
}

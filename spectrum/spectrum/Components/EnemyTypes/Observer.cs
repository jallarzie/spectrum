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
        public Observer(Color tint, Vector2 position, Entity2D target)
            : base(tint, position, target)
        {
            Scale = 0.2f;
            Path = new DistantFollow(this, target, 500);
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
                return new Laser(Tint, Position, direction, 300f, LaserAlignment.Enemy);
            }

            return null;
        }
    }
}

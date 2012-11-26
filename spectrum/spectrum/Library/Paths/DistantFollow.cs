using System;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;

namespace Spectrum.Library.Paths
{
    class DistantFollow : Follow
    {
        protected int targetDistance;
        protected int leeway;
        protected bool moving;

        public DistantFollow(CoordinateSystem entity, Entity2D target, int distance)
            : base(entity, target)
        {
            targetDistance = distance;
            leeway = 0;
            moving = false;
        }

        public DistantFollow(CoordinateSystem entity, Entity2D target, int distance, int leeway)
            : base(entity, target)
        {
            targetDistance = distance;
            this.leeway = leeway;
            moving = false;
        }

        public override Vector2 Move(float distance)
        {
            float currentDistance = Vector2.Distance(Target.Position, Position);

            if ((moving && currentDistance > targetDistance) ||
                !moving && currentDistance > targetDistance + leeway)
            {
                Direction = Target.Position - Position;
                Direction.Normalize();
                moving = true;
                return base.Move(distance);
            }
            else
            {
                moving = false;
            }

            return base.Move(0);
        }
    }
}

using System;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;

namespace Spectrum.Library.Paths
{
    class DistantFollow : Follow
    {
        private int targetDistance;

        public DistantFollow(CoordinateSystem entity, Entity2D target, int distance)
            : base(entity, target)
        {
            targetDistance = distance;
        }

        public override Vector2 Move(float distance)
        {
            if (Vector2.Distance(Target.Position, Position) > targetDistance)
            {
                Direction = Target.Position - Position;
                Direction.Normalize();
                return base.Move(distance);
            }

            return base.Move(0);
        }
    }
}

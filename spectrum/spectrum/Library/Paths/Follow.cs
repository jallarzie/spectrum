using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using System;
using Spectrum.Library.Graphics;

namespace Spectrum.Library.Paths
{
    class Follow : Linear
    {
        public Follow(CoordinateSystem entity, Entity2D target) : base(entity)
        {
            Target = target;
        }

        public override Vector2 Move(float distance)
        {
            Direction = Target.Position - Position;
            Direction.Normalize();
            return base.Move(distance);
        }

        private Entity2D Target;
    }
}

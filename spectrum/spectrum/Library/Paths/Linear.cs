using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using System;

namespace Spectrum.Library.Paths
{
    public class Linear : Path
    {
        public Linear(Vector2 position = new Vector2(), Vector2 direction = new Vector2())
        {
            PathAwareEntity = null;
            Position = position;
            if (direction.X != 0 || direction.Y != 0)
            {
                direction.Normalize();
            }
            Direction = direction;
        }

        public Linear(CoordinateSystem entity, Vector2 direction = new Vector2())
        {
            PathAwareEntity = entity as PathAware;
            Position = entity.Position;
            if (direction.X != 0 || direction.Y != 0)
            {
                direction.Normalize();
            }
            Direction = direction;
        }

        public Vector2 Move(float distance)
        {
            Position += Direction * distance;

            if (PathAwareEntity != null)
            {
                PathAwareEntity.PathPosition(Position);
                PathAwareEntity.PathDirection((float)Math.Atan2(Direction.X, -Direction.Y));
            }

            return Position;
        }

        public void Collide(Vector2 normal, Vector2 position)
        {
            Position = position;
        }

        private PathAware PathAwareEntity;

        private Vector2 Position;
        private Vector2 Direction;
    }
}

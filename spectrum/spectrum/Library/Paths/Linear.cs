using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using System;

namespace Spectrum.Library.Paths
{
    public class Linear : Path
    {
        public Linear() : this(new Vector2(), new Vector2()) { }

        public Linear(Vector2 position , Vector2 direction)
        {
            PathAwareEntity = null;
            Position = position;
            if (direction.X != 0 || direction.Y != 0)
            {
                direction.Normalize();
            }
            Direction = direction;
        }

        public Linear(CoordinateSystem entity) : this(entity, new Vector2()) { }

        public Linear(CoordinateSystem entity, Vector2 direction)
        {
            PathAwareEntity = entity as PathAware;
            Position = entity.Position;
            if (direction.X != 0 || direction.Y != 0)
            {
                direction.Normalize();
            }
            Direction = direction;
        }

        public virtual Vector2 Move(float distance)
        {
            Position += Direction * distance;

            if (PathAwareEntity != null)
            {
                PathAwareEntity.PathPosition(Position);
                PathAwareEntity.PathDirection((float)Math.Atan2(Direction.X, -Direction.Y));
            }

            return Position;
        }

        /// <summary>
        /// Recoil away form a position
        /// </summary>
        /// <param name="position"></param>
        public virtual Vector2 Recoil(Vector2 position, float distance)
        {
            Vector2 recoil = Position - position;
            recoil.Normalize();

            Position += distance * recoil;

            if (PathAwareEntity != null)
            {
                PathAwareEntity.PathPosition(Position);
            }

            return Position;
        }

        public void Collide(Vector2 normal, Vector2 position)
        {
            Position = position;
        }

        protected PathAware PathAwareEntity;

        protected Vector2 Position;
        protected Vector2 Direction;
    }
}

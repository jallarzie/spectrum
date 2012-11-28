using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using Spectrum.Components;
using System;

namespace Spectrum.Library.Paths
{
    public class User : Path
    {
        /*public User()
            : this(new Vector2()) { }*/

        public User(Vector2 position, Rectangle screenBounds)
        {
            PathAwareEntity = null;
            Position = position;
            this.screenBounds = screenBounds;
        }

        public User(CoordinateSystem entity, Rectangle screenBounds)
        {
            PathAwareEntity = entity as PathAware;
            Position = entity.Position;
            this.screenBounds = screenBounds;
        }

        public Vector2 Move(float distance)
        {
            Vector2 direction = InputController.Instance.GetMovingDirection(((Ship)PathAwareEntity).PlayerIndex);

            if (direction.X == 0 && direction.Y == 0)
                return Position;
            if (direction.LengthSquared() > 1)
                direction.Normalize();

            Position += direction * distance;

            if (!screenBounds.Contains((int)Position.X, (int)Position.Y))
            {
                Position.X = MathHelper.Clamp(Position.X, screenBounds.X, screenBounds.Width);
                Position.Y = MathHelper.Clamp(Position.Y, screenBounds.Y, screenBounds.Height);
            }

            if (PathAwareEntity != null)
            {
                PathAwareEntity.PathPosition(Position);
                PathAwareEntity.PathDirection((float) Math.Atan2(direction.X, - direction.Y));
            }

            return Position;
        }

        /// <summary>
        /// Recoil away form a position
        /// </summary>
        /// <param name="position"></param>
        public Vector2 Recoil(Vector2 position, float distance)
        {
            Vector2 recoil = Position - position;
            recoil.Normalize();

            Position += distance * recoil;

            if (!screenBounds.Contains((int)Position.X, (int)Position.Y))
            {
                Position.X = MathHelper.Clamp(Position.X, screenBounds.X, screenBounds.Width);
                Position.Y = MathHelper.Clamp(Position.Y, screenBounds.Y, screenBounds.Height);
            }

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

        private PathAware PathAwareEntity;
        private Rectangle screenBounds;
        private Vector2 Position;
    }
}

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using System;

namespace Spectrum.Library.Paths
{
    public class User : Path
    {
        public User()
            : this(new Vector2()) { }

        public User(Vector2 position)
        {
            PathAwareEntity = null;
            Position = position;
        }

        public User(CoordinateSystem entity)
        {
            PathAwareEntity = entity as PathAware;
            Position = entity.Position;
        }

        public Vector2 Move(float distance)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            Vector2 direction = new Vector2();

            if (gamepadState.IsConnected)
            {
                if (gamepadState.ThumbSticks.Left.LengthSquared() != 0)
                {
                    direction = gamepadState.ThumbSticks.Left;
                    direction.Y *= -1;
                }
                else
                {
                    if (gamepadState.IsButtonDown(Buttons.DPadLeft))
                        direction.X -= 1;
                    if (gamepadState.IsButtonDown(Buttons.DPadRight))
                        direction.X += 1;
                    if (gamepadState.IsButtonDown(Buttons.DPadUp))
                        direction.Y -= 1;
                    if (gamepadState.IsButtonDown(Buttons.DPadDown))
                        direction.Y += 1;
                }
            }
            else
            {
                if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
                    direction.X -= 1;
                if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
                    direction.X += 1;
                if (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W))
                    direction.Y -= 1;
                if (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S))
                    direction.Y += 1;
            }

            if (direction.X == 0 && direction.Y == 0)
                return Position;

            direction.Normalize();

            Position += direction * distance;

            if (PathAwareEntity != null)
            {
                PathAwareEntity.PathPosition(Position);
                PathAwareEntity.PathDirection((float) Math.Atan2(direction.X, - direction.Y));
            }

            return Position;
        }

        public void Collide(Vector2 normal, Vector2 position)
        {
            Position = position;
        }

        private PathAware PathAwareEntity;

        private Vector2 Position;
    }
}

﻿using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using System;

namespace Spectrum.Library.Paths
{
    public class User : Path
    {
        public User(Vector2 position = new Vector2())
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
            Vector2 direction = new Vector2();

            if (keyboardState.IsKeyDown(Keys.Left))
                direction.X -= 1;
            if (keyboardState.IsKeyDown(Keys.Right))
                direction.X += 1;
            if (keyboardState.IsKeyDown(Keys.Up))
                direction.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                direction.Y += 1;

            if (direction.X == 0 && direction.Y == 0)
                return Position;

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
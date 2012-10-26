using Microsoft.Xna.Framework;
using System;

namespace Spectrum.Library.Geometry
{
    public interface CoordinateSystem
    {
        CoordinateSystem Parent { get; }
        Vector2 Position { get; }
        float Rotation { get; }
        float Scale { get; }
    }

    public static class CoordinateSystemImplementation
    {
        public static Vector2 WorldPosition(this CoordinateSystem system)
        {
            if (system.Parent == null)
                return system.Position;

            Vector2 parentWorldPosition = system.Parent.WorldPosition();
            float parentWorldRotation = system.Parent.WorldRotation();

            Vector2 position = system.Position;

            return parentWorldPosition + new Vector2(
                (float) (position.X * Math.Cos(parentWorldRotation) - position.Y * Math.Sin(parentWorldRotation)),
                (float) (position.X * Math.Sin(parentWorldRotation) + position.Y * Math.Cos(parentWorldRotation))
            );
        }

        public static float WorldRotation(this CoordinateSystem system)
        {
            if (system.Parent == null)
                return system.Rotation;

            float parentWorldRotation = system.Parent.WorldRotation();
            float rotation = system.Rotation;

            return parentWorldRotation + rotation;
        }

        public static float WorldScale(this CoordinateSystem system)
        {
            if (system.Parent == null)
                return system.Scale;

            float parentWorldScale = system.Parent.WorldScale();
            float scale = system.Scale;

            return parentWorldScale * scale;
        }
    }
}

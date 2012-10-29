using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;

namespace Spectrum.Components
{
    public class Laser : Sprite, PathAware
    {
        public Laser(Color tint, Vector2 position, Vector2 direction) : base("laser")
        {
            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 0.4f;
            Tint = tint;
            Position = position;
            Path = new Linear(this, direction);
        }

        public void PathPosition(Vector2 position)
        {
            Position = position;
        }

        public void PathDirection(float angle)
        {
            Rotation = angle;
        }

        public bool IsVisible(Rectangle window)
        {
            return (Position.X + Width / 2 >= window.X &&
                    Position.X - Width / 2 < window.Width &&
                    Position.Y + Height / 2 >= window.Y &&
                    Position.Y - Height / 2 < window.Height);
        }

        public Color Color { get; set; }

        public Path Path;
    }
}

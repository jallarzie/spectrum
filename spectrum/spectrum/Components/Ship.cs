using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;

namespace Spectrum.Components
{
    public class Ship : Sprite, PathAware
    {
        public Ship() : base("ship")
        {
            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 0.3f;
            setTint(Color.Green);
        }

        public void setTint(Color color)
        {
            if (color == Color.Red ||
                color == Color.Green ||
                color == Color.Blue ||
                color == Color.Cyan ||
                color == Color.Magenta ||
                color == Color.Yellow ||
                color == Color.White)
            {
                Tint = color;
            }
            else
            {
                Tint = new Color(90, 90, 90); // looks better than Color.Black with sprite tinting
            }
        }

        public void PathPosition(Vector2 position)
        {
            Position = position;
        }

        public void PathDirection(float angle)
        {
            Rotation = angle;
        }

        public Path Path;
    }
}

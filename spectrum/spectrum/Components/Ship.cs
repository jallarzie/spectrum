using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;

namespace Spectrum.Components
{
    public class Ship : Sprite, PathAware
    {
        public Ship() : base("ship")
        {
            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 0.3f;
            SetTint(Color.Black);
        }

        protected void SetTint(Color color)
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
            else if (color.R == 0 && color.G == 255 && color.B == 0)
            {
                Tint = Color.Green;
            }
            else
            {
                Tint = new Color(90, 90, 90); // looks better than Color.Black with sprite tinting
            }
        }

        public void AbsorbTint(Color color)
        {
            Color combined = Color.Black;
            // use ^ so absorbed colors are lost if already present
            // use || so absorbed colors are always contained in new color
            if (color.R > 100 || Tint.R > 100) combined.R = 255;
            if (color.G > 100 || Tint.G > 100) combined.G = 255;
            if (color.B > 100 || Tint.B > 100) combined.B = 255;
            SetTint(combined);
        }

        public void LoseTint(Color color)
        {
            Color combined = Color.Black;
            if (color.R <= 100 && Tint.R > 100) combined.R = 255;
            if (color.G <= 100 && Tint.G > 100) combined.G = 255;
            if (color.B <= 100 && Tint.B > 100) combined.B = 255;
            SetTint(combined);
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

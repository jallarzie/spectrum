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
            Scale = 0.2f;
            SetTint(Color.Black);
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

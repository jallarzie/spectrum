using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;

namespace Spectrum.Components
{
    public class Powerup : Sprite
    {
        public Powerup(Color tint, Vector2 position) : base("powerup")
        {
            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 1f;
            Tint = tint;
            Position = position;
        }
    }
}

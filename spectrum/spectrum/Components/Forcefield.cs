using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;

namespace Spectrum.Components
{
    class Forcefield : Sprite
    {
        public Forcefield(Vector2 position, Color tint) : base("forcefield")
        {
            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 0.2f;
            Tint = tint;
            Position = position;

            Health = 1;
        }

        public int Health;
    }
}

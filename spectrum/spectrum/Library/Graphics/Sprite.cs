using Microsoft.Xna.Framework.Graphics;
using Spectrum.Library.Geometry;

namespace Spectrum.Library.Graphics
{
    public class Sprite : Entity2D
    {
        public Sprite(string name) : this(name, null) { }

        public Sprite(string name, CoordinateSystem parent)
            : base(Application.Instance.Content.Load<Texture2D>(name), parent)
        {
        }
    }
}

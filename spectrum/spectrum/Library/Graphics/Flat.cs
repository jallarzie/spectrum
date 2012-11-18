using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;

namespace Spectrum.Library.Graphics
{
    class Flat : Entity2D
    {
        public Flat(Vector2 size, Color color, CoordinateSystem parent)
            : base(null, parent)
        {
            Dirty = true;
            Size = size;
            Color = color;
        }

        public override void Update()
        {
            int width = (int) Width;
            int height = (int) Height;

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
                data[i] = _Color;

            Texture = new Microsoft.Xna.Framework.Graphics.Texture2D(Application.Instance.GraphicsDevice, width, height);
            Texture.SetData(data);
        }

        private Color _Color;
        public Color Color
        {
            get
            {
                return _Color;
            }

            set
            {
                _Color = value;
                Dirty = true;
            }
        }

        private Vector2 _Size;
        public Vector2 Size
        {
            get
            {
                return _Size;
            }

            set
            {
                _Size = value;
                Dirty = true;
            }
        }

        public new float Width
        {
            get
            {
                return _Size.X;
            }

            set
            {
                Size = new Vector2(value, _Size.Y);
            }
        }

        public new float Height
        {
            get
            {
                return _Size.Y;
            }

            set
            {
                Size = new Vector2(_Size.Y, value);
            }
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;

namespace Spectrum.Library.Graphics
{
    public class Entity2D : Drawable, CoordinateSystem
    {
        public Entity2D(Texture2D texture) : this(texture, null) { }

        public Entity2D(Texture2D texture, CoordinateSystem parent)
        {
            Dirty = false;
            Texture = texture;

            Parent = parent;
            Position = new Vector2(0, 0);
            Rotation = 0f;
            Scale = 1f;
            Tint = Color.White;

            Origin = new Vector2(0, 0);
            Layer = 0f;
            Opacity = 1f;
            Flip = SpriteEffects.None;
        }

        public int Width
        {
            get
            {
                if (Dirty) this.Update();
                return Texture.Width;
            }
        }

        public int Height
        {
            get
            {
                if (Dirty) this.Update();
                return Texture.Height;
            }
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            if (Dirty) this.Update();
            targetSpriteBatch.Draw(Texture, this.WorldPosition(), null, Tint * Opacity, this.WorldRotation(), Origin, Scale, Flip, Layer);
        }

        protected void SetTint(Color color)
        {
            if (color == Color.Red ||
                color == Color.Lime ||
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

        protected Texture2D Texture;
        protected bool Dirty;

        public CoordinateSystem Parent { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Color Tint { get; set; }

        public Vector2 Origin;
        public float Layer;
        public float Opacity;
        public SpriteEffects Flip;
    }
}

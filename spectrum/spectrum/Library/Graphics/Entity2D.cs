using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;

namespace Spectrum.Library.Graphics
{
    public class Entity2D : Drawable, CoordinateSystem
    {
        public Entity2D(Texture2D texture, CoordinateSystem parent = null)
        {
            Dirty = false;
            Texture = texture;

            Parent = parent;
            Position = new Vector2(0, 0);
            Rotation = 0f;
            Scale = 1f;

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
            targetSpriteBatch.Draw(Texture, this.WorldPosition(), null, Color.White * Opacity, this.WorldRotation(), Origin, Scale, Flip, Layer);
        }

        protected Texture2D Texture;
        protected bool Dirty;

        public CoordinateSystem Parent { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }

        public Vector2 Origin;
        public float Layer;
        public float Opacity;
        public SpriteEffects Flip;
    }
}

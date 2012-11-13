using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;
using Spectrum.Library.Collisions;

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
            BoundingArea = new Sphere(new Vector2(0, 0), 1);
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

        protected Texture2D Texture;
        protected bool Dirty;

        public CoordinateSystem Parent { get; set; }
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                BoundingArea.Shape.Center = value;
            }
        }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Color Tint { get; set; }
        public Area BoundingArea { get; protected set; }

        public Vector2 Origin;
        public float Layer;
        public float Opacity;
        public SpriteEffects Flip;
    }
}

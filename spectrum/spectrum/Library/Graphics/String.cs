using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Geometry;

namespace Spectrum.Library.Graphics
{
    public class String : Drawable
    {
        public String(string fontName, string text)
        {
            Font = Application.Instance.Content.Load<SpriteFont>(fontName);
            Text = text;

            Origin = new Vector2(0, 0);
            Position = new Vector2(0, 0);
        }

        public float Width
        {
            get
            {
                return Font.MeasureString(Text).X;
            }
        }

        public float Height
        {
            get
            {
                return Font.MeasureString(Text).Y;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            targetSpriteBatch.DrawString(Font, Text, Position, Color.White * Opacity, Rotation, Origin, Scale, Flip, Layer);
        }

        public SpriteFont Font;
        public string Text;

        public Vector2 Origin;
        public Vector2 Position;

        public float Opacity = 1f;
        public float Rotation = 0f;
        public float Scale = 1f;

        public float Layer = 0f;

        public SpriteEffects Flip = SpriteEffects.None;
    }
}

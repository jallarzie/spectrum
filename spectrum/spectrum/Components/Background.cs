using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spectrum.Components
{
    /// <summary>
    /// Component That Draws the Background
    /// </summary>
    public class Background : Spectrum.Library.Graphics.Drawable
    {
        /// <summary>
        /// The SourceRectangle of the dust-like texture.
        /// </summary>
        private static readonly Rectangle DUST_SRC_RECTANGLE = new Rectangle(255, 702, 128, 128);

        private Texture2D texture;

        public Background() 
        {
            texture = Application.Instance.Content.Load<Texture2D>("background");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Viewport viewPort = Application.Instance.GraphicsDevice.Viewport;

            // Draw the Texture until it fills the entire ViewPort
            for (int i = 0; i < (int)Math.Ceiling((double)viewPort.Width/DUST_SRC_RECTANGLE.Width); i++)
            {
                for (int j = 0; j < (int)Math.Ceiling((double)viewPort.Height / DUST_SRC_RECTANGLE.Height); j++)
                {
                    spriteBatch.Draw(
                        texture: texture,
                        position: new Vector2(i * DUST_SRC_RECTANGLE.Width, j * DUST_SRC_RECTANGLE.Height),
                        sourceRectangle: DUST_SRC_RECTANGLE,
                        color: Color.White,
                        rotation: 0,
                        origin: Vector2.Zero,
                        scale: 1.0f,
                        effects: SpriteEffects.None,
                        layerDepth: 1.0f);
                }
            }
        }
    }
}

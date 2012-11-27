using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using System.Collections.Generic;
using System;

namespace Spectrum.Components
{
    /// <summary>
    /// Component That Draws the Background
    /// </summary>
    public class Background : Sprite
    {
        private static readonly Rectangle STAR_SRC = new Rectangle(351, 21, 128, 170);

        private class Star
        {
            public Color color;
            public float scale;
            public Vector2 position;
            public float depth;

            public Star(Color color, float scale, Vector2 position, float depth)
            {
                this.color = color;
                this.scale = scale / depth;
                this.position = position;
                this.depth = depth;
            }
        }

        private List<Star> Stars;

        public Background(int nbStars, Random RNG) : base("particles")
        {
            Viewport viewPort = Application.Instance.GraphicsDevice.Viewport;
            Stars = new List<Star>();
            Layer = Layers.Background;
            for (int i = 0; i < nbStars; i++)
            {
                Color color = Color.White;
                color.R -= (byte)RNG.Next(50);
                color.G -= (byte)RNG.Next(50);
                color.B -= (byte)RNG.Next(50);
                color.A -= (byte)RNG.Next(200); // why does this do nothing?
                Star star = new Star(color,
                                     3f, // to get more small stars
                                     new Vector2(RNG.Next(viewPort.Width), RNG.Next(viewPort.Height)),
                                     (float) RNG.NextDouble() * 3 + 1);
                Stars.Add(star);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Star star in Stars)
            {
                star.position.X -= (float) (1f * (10f - star.depth)) / 20;
                if (star.position.X < 0)
                    star.position.X = Application.Instance.GraphicsDevice.Viewport.Width;
                spriteBatch.Draw(Texture, star.position, STAR_SRC, star.color, Rotation, Origin, star.scale * 0.02f, Flip, Layer);
            }
        }
    }
}

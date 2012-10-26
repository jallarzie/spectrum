using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Spectrum.Library.Graphics
{
    public class DisplayList : Drawable
    {
        public DisplayList()
        {
            List = new List<Drawable>();
        }

        public void Add(Drawable drawable)
        {
            List.Add(drawable);
        }

        public void Remove(Drawable drawable)
        {
            List.Remove(drawable);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Drawable drawable in List)
            {
                drawable.Draw(gameTime, spriteBatch);
            }
        }

        private List<Drawable> List;
    }
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spectrum.Library.Graphics
{
    public interface Drawable
    {
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}

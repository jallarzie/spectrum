using Microsoft.Xna.Framework;

namespace Spectrum.Library.States
{
    public class State
    {
        public virtual void Initialize()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual State Transition()
        {
            return null;
        }

        public virtual void Destroy()
        {
        }
    }
}

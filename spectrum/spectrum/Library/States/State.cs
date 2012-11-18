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

        public virtual bool Transition()
        {
            return false;
        }

        public virtual void Destroy()
        {
        }
    }
}

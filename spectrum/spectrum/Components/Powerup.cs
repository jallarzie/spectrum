using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;

namespace Spectrum.Components
{
    public class Powerup : Sprite
    {
        private const int LIFESPAN = 10000;
        private const int BLINK_RANGE = 3000;
        private const int BLINK_INTERVAL = 300;

        public int TimeToLive { get; protected set; }

        public Powerup(Color tint, Vector2 position, float layer) : base("powerup")
        {
            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 1f;
            Tint = tint;
            Layer = layer;
            Position = position;
            TimeToLive = LIFESPAN;
        }

        public void UpdateLifespan(GameTime gameTime)
        {
            TimeToLive -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if (TimeToLive < BLINK_RANGE)
            {
                Opacity = TimeToLive % BLINK_INTERVAL / (float)BLINK_INTERVAL;
            }
        }
    }
}

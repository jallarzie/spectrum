using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spectrum.Components
{
    public class PowerCore : Sprite
    {
        private static readonly int TEXTURE_HEIGHT = 129;
        private static readonly int TEXTURE_WIDTH = 129;

        /// <summary>
        /// The interval at which the Core Regains Health (in ms).
        /// </summary>
        private static readonly int REGEN_INTERVAL = 3000;

        /// <summary>
        /// The Amount Health the Core Regains at each Health Regeneration
        /// </summary>
        private static readonly float REGEN_RATE = 0.1f;

        public PowerCore() : base("powercore")
        {
            Health = 0.1f;

            // Place the PowerCore at the center of the ViewPort
            Viewport viewPort = Application.Instance.GraphicsDevice.Viewport;
            Position = new Vector2(viewPort.Width/2, viewPort.Height/2);
            Origin = new Vector2(TEXTURE_WIDTH / 2, TEXTURE_HEIGHT / 2);
        }
        
        public void Update(GameTime gameTime)        
        {
            TimeElapsedSinceLastRegen = TimeElapsedSinceLastRegen.Add(gameTime.ElapsedGameTime);
            if (TimeElapsedSinceLastRegen.TotalMilliseconds > REGEN_INTERVAL) 
            {
                RegainHealth();
                TimeElapsedSinceLastRegen = new TimeSpan(0);
            }

            Scale = Health + 1.0f;
        }

        /// <summary>
        /// Increast the Power Core's health
        /// </summary>
        private void RegainHealth() 
        {
            Health = Health + REGEN_RATE;
        }

        /// <summary>
        /// Current Health of the Power Core (in % between 0 and 1)
        /// </summary>
        public float Health;

        private TimeSpan TimeElapsedSinceLastRegen = new TimeSpan(0);
    }
}

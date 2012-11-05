using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spectrum.Library.Collisions;

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
        /// The amount of HP the Core Regains at each Health Regeneration
        /// </summary>
        private static readonly int REGEN_RATE = 1;

        public PowerCore() : base("powercore")
        {
            Health = 1;
            TimeElapsedSinceLastRegen = new TimeSpan(0);

            // Place the PowerCore at the center of the ViewPort
            Viewport viewPort = Application.Instance.GraphicsDevice.Viewport;
            Position = new Vector2(viewPort.Width/2, viewPort.Height/2);
            Origin = new Vector2(TEXTURE_WIDTH / 2, TEXTURE_HEIGHT / 2);

            BoundingSphere = new Sphere(Position, CalculateCurrentRadius());
        }
        
        public void Update(GameTime gameTime)        
        {
            TimeElapsedSinceLastRegen = TimeElapsedSinceLastRegen.Add(gameTime.ElapsedGameTime);
            if (TimeElapsedSinceLastRegen.TotalMilliseconds > REGEN_INTERVAL) 
            {
                RegainHealth();
                TimeElapsedSinceLastRegen = new TimeSpan(0);
            }

            Scale = CalculateCurrentScale();
            BoundingSphere.Circle.ChangeRadius(CalculateCurrentRadius(), CalculateCurrentRadius());
        }

        public Sphere GetBoundingSphere() 
        {
            return BoundingSphere;
        }

        /// <summary>
        /// Returns the current Health of the Power Core (in HP)
        /// </summary>
        public int GetHealth() 
        {
            return Health;
        }

        /// <summary>
        /// Decrease the Core's health by the passed Damage.
        /// The health will always be >= 0;
        /// </summary>
        /// <param name="Damage">The Damage to decrease the health by (in HP).</param>
        public void DecreaseHealthBy(int damage) 
        {
            Health = (int)MathHelper.Clamp(Health - damage, 0, Health - damage);

            if (Health == 0 && Observer != null) 
            {
                Observer.OnPowerCoreHealthReachedZero();
            }
        }

        /// <summary>
        /// Returns the new radius of the Core's Bounding Sphere.
        /// </summary>
        /// <returns></returns>
        private float CalculateCurrentRadius()
        {
            return TEXTURE_HEIGHT / 2 * CalculateCurrentScale();
        }

        /// <summary>
        /// Returns the scale Factor to apply on the core so that it reflects its health.
        /// </summary>
        /// <returns></returns>
        private float CalculateCurrentScale()
        {
            return Health/10f + 1.0f;
        }

        /// <summary>
        /// Increase the Power Core's health
        /// </summary>
        private void RegainHealth() 
        {
            Health = Health + REGEN_RATE;
        }

        /// <summary>
        /// This object's observer
        /// </summary>
        public EventObservers.PowerCoreObserver Observer;

        private int Health;

        private Sphere BoundingSphere;
        private TimeSpan TimeElapsedSinceLastRegen;
    }
}

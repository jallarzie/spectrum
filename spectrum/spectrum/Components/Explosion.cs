using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;

namespace Spectrum.Components
{
    class Explosion : Sprite
    {
        /// <summary>
        /// Entity that is exploding
        /// </summary>
        private Entity2D ExplodingEntity;

        public Explosion(Entity2D explodingEntity)
            : base("smallExplosion")
        {
            ExplodingEntity = explodingEntity;
        }

        public override void Update()
        {
            base.Update();
        }
    }
}

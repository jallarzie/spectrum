using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework;

namespace Spectrum.Components
{
    public class ThumbStickInnerCircleSprite : Sprite
    {
        public static readonly int CIRCLE_RADIUS = 7;

        public ThumbStickInnerCircleSprite(Vector2 position) : base("thumbstick-circle-inner") 
        {
            Position = position;
        }

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, CIRCLE_RADIUS * 2, CIRCLE_RADIUS * 2);
        }
    }
}

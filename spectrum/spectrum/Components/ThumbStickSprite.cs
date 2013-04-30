using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework;

namespace Spectrum.Components
{
    public class ThumbStickSprite : Sprite
    {
        public static readonly int CIRCLE_RADIUS = 25;

        public ThumbStickInnerCircleSprite ThumbStickInnerCircleSprite { get; private set; }

        public ThumbStickSprite(Vector2 position) : base("thumbstick-circle") 
        {
            Position = position;
            ThumbStickInnerCircleSprite = new ThumbStickInnerCircleSprite(position);
        }

        public void Update(PhoneThumbsticController thumbStickController, Boolean isShipMoving)
        {
            Vector2 centerPosition = new Vector2(GetRectangle().Center.X, GetRectangle().Center.Y);

            if (!isShipMoving) 
            {
                ThumbStickInnerCircleSprite.Position = centerPosition - new Vector2(ThumbStickInnerCircleSprite.CIRCLE_RADIUS, ThumbStickInnerCircleSprite.CIRCLE_RADIUS);
            }
            else if (thumbStickController.LastMovedLocation != null)
            {
                Vector2 LastMovedPosition = (Vector2)thumbStickController.LastMovedLocation;
                Vector2 DistanceFromLastMovedPosition = LastMovedPosition - centerPosition;

                if (DistanceFromLastMovedPosition.Length() <= CIRCLE_RADIUS)
                {
                    ThumbStickInnerCircleSprite.Position = LastMovedPosition - new Vector2(ThumbStickInnerCircleSprite.CIRCLE_RADIUS, ThumbStickInnerCircleSprite.CIRCLE_RADIUS);
                }
                else
                {
                    DistanceFromLastMovedPosition.Normalize();
                    ThumbStickInnerCircleSprite.Position = centerPosition + CIRCLE_RADIUS * DistanceFromLastMovedPosition - new Vector2(ThumbStickInnerCircleSprite.CIRCLE_RADIUS, ThumbStickInnerCircleSprite.CIRCLE_RADIUS);
                }
            }
        }

        public Rectangle GetRectangle() 
        {
            return new Rectangle((int)Position.X, (int)Position.Y, CIRCLE_RADIUS*2, CIRCLE_RADIUS*2);
        }
    }
}

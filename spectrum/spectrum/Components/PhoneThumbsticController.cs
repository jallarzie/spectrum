using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Spectrum.Components
{
    /// <summary>
    /// Creates Thumbstick controls over a defined 
    /// Rectangular Area.
    /// </summary>
    public class PhoneThumbsticController
    {
        private Rectangle ListeningArea { get; set; }

        /// <summary>
        /// The Position of the last TouchLocation that had
        /// a Pressed TouchLocationState.
        /// </summary>
        public Vector2? LastPressedLocation { get; private set; }

        /// <summary>
        /// The Position of the last TouchLocation that had
        /// a Moved TouchLocationState.
        /// </summary>
        public Vector2? LastMovedLocation { get; private set; }

        public PhoneThumbsticController(Rectangle listeningArea) 
        {
            ListeningArea = listeningArea;

            LastPressedLocation = null;
            LastMovedLocation = null;
        }

        public void Update(TouchCollection touchLocationState) 
        {
            foreach (TouchLocation touchLocation in touchLocationState)
            {
                switch (touchLocation.State)
                {
                    case TouchLocationState.Invalid:
                        break;
                    case TouchLocationState.Moved:
                        if (LastPressedLocation != null
                            && getRectangleFromPoint(touchLocation.Position).Intersects(ListeningArea)) 
                        {
                            LastMovedLocation = touchLocation.Position;
                        }
                        break;
                    case TouchLocationState.Pressed:
                        if (getRectangleFromPoint(touchLocation.Position).Intersects(ListeningArea)) 
                        {
                            this.LastPressedLocation = touchLocation.Position;
                        }
                        break;
                    case TouchLocationState.Released:
                        LastPressedLocation = null;
                        LastMovedLocation = null;
                        break;
                }
            }
        }

        public Vector2 GetMovingDirection() 
        {
            if (LastPressedLocation != null
                && LastMovedLocation != null)
            {
                return (Vector2)(LastMovedLocation - LastPressedLocation);
            }
            else 
            {
                return Vector2.Zero;
            }
        }

        public static Rectangle getRectangleFromPoint(Vector2 point)
        {
            return new Rectangle(
                        (int)point.X,
                        (int)point.Y,
                        1,
                        1
                    );
        }
    }
}

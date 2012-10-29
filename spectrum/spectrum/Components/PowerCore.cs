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

        public PowerCore() : base("powercore")
        {
            // Place the PowerCore at the center of the ViewPort
            Viewport viewPort = Application.Instance.GraphicsDevice.Viewport;
            Position = new Vector2(viewPort.Width/2 - TEXTURE_WIDTH/2, viewPort.Height/2 - TEXTURE_HEIGHT/2);
        }
    }
}

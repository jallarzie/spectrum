using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework;

namespace Spectrum.Components
{
    class Crosshair : Sprite
    {
        public Crosshair() : base("crosshair")
        {
            Layer = Layers.Hud;
            Origin = new Vector2(7, 7);
            Position = new Vector2(-100, -100);
        }

    }
}

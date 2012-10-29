using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;
using System.Collections.Generic;
using System;

namespace Spectrum.Components
{
    public class Enemy : Ship
    {
        public Enemy(Color tint, Vector2 position, Entity2D target)
        {
            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 0.2f;
            SetTint(tint);
            Position = position;
            Path = new Follow(this, target);
        }

        public Powerup DropPowerup(Color excluded, Random RNG)
        {
            List<Color> possibleColors = new List<Color>();
            if (excluded.R <= 100 && Tint.R > 100)
            {
                possibleColors.Add(Color.Red);
            }
            if (excluded.G <= 100 && Tint.G > 100)
            {
                possibleColors.Add(Color.Green);
            }
            if (excluded.B <= 100 && Tint.B > 100)
            {
                possibleColors.Add(Color.Blue);
            }

            if (possibleColors.Count > 0) return new Powerup(possibleColors[RNG.Next(possibleColors.Count)], Position);
            else return new Powerup(Color.Black, Position); // using black as a null value
        }
    }
}

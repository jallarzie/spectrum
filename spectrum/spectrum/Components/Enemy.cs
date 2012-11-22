using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;
using System.Collections.Generic;
using System;

namespace Spectrum.Components
{
    public abstract class Enemy : Sprite, PathAware
    {
        public Entity2D Target { get; protected set; }
        public float Speed { get; protected set; }
        public float FireRate { get; protected set; }
        public float LastFired { get; protected set; }
        
        public Enemy(string textureName, Color tint, Vector2 position, Entity2D target) : base(textureName)
        {
            Origin = new Vector2(Width / 2, Height / 2);           
            SetTint(tint);
            Position = position;
            Speed = 0f;
            FireRate = 0f;
            Target = target;
        }

        public Powerup DropPowerup(Color excluded, Random RNG)
        {
            List<Color> possibleColors = new List<Color>();

            if (excluded.R <= 200 && Tint.R > 200) possibleColors.Add(Color.Red);
            if (excluded.G <= 200 && Tint.G > 200) possibleColors.Add(Color.Lime);
            if (excluded.B <= 200 && Tint.B > 200) possibleColors.Add(Color.Blue);

            if (possibleColors.Count > 0) return new Powerup(possibleColors[RNG.Next(possibleColors.Count)], Position);
            else return new Powerup(Color.Black, Position); // using black as a null value
        }

        public void PathPosition(Vector2 position)
        {
            Position = position;
        }

        public void PathDirection(float angle)
        {
            Rotation = angle;
        }

        public Path Path;

        public abstract Laser Attack(float seconds);

        /// <summary>
        /// Returns the Ship's Score Value.
        /// </summary>
        public abstract int GetScoreValue();

        /// <summary>
        /// Returns the Explosion to display when the object is hit.
        /// </summary>
        /// <param name="destroyedTime">the time at which the object gets destroyed.</param>
        /// <returns></returns>
        public abstract Explosion GetExplosion(double destroyedTime);
    }
}

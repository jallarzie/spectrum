﻿using Microsoft.Xna.Framework.Graphics;
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
        private static float layerModifier = 0.0f;

        public Entity2D Target { get; protected set; }
        public float Speed { get; protected set; }
        public float FireRate { get; protected set; }
        public float LastFired { get; protected set; }
        
        public Enemy(string textureName, Color tint, Vector2 position, Entity2D target) : base(textureName)
        {
            HealthBar = new HealthBar(this, "");
            Origin = new Vector2(Width / 2, Height / 2);
            SetTint(tint);
            Position = position;
            Layer = Layers.Enemies - layerModifier;
            layerModifier += 0.00001f;
            if (layerModifier > 0.01f)
                layerModifier = 0.0f;
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

            if (possibleColors.Count > 0) return new Powerup(possibleColors[RNG.Next(possibleColors.Count)], Position, Layer + 0.01f);
            else return new Powerup(Color.Black, Position, Layer + 0.01f); // using black as a null value
        }

        public void PathPosition(Vector2 position)
        {
            Position = position;
        }

        public void PathDirection(float angle)
        {
            Rotation = angle;
        }

        public void ChangeTarget(Entity2D target)
        {
            if (target != Target)
            {
                Target = target;
                Path.Target = target;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            base.Draw(gameTime, targetSpriteBatch);
            HealthBar.Draw(gameTime, targetSpriteBatch);
        }

        public Follow Path;
        public HealthBar HealthBar;

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

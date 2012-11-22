using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spectrum.Components
{
    public class HealthBar : Sprite
    {
        private static readonly Color NEGATIVE_SPACE_COLOR = Color.Gray;
        private static readonly Color HEALTH_BAR_COLOR = Color.LightSeaGreen;

        public HealthBar(Ship entity) : base("HealthBar") 
        {
            Ship = entity;
            Position = Ship.Position;
            //CurrentHealth = Ship.
        }

        public void Update(GameTime gameTime) 
        {
            Position = Ship.Position;            
        }

        public override void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            // Draw the negative space for the health bar
            targetSpriteBatch.Draw(Texture, Position, NegativeSpaceRectangle, NEGATIVE_SPACE_COLOR);

            //Draw the current health level based on the current Health
            targetSpriteBatch.Draw(Texture, Position, PositiveSpaceRectangle, HEALTH_BAR_COLOR);

            //Draw the box around the health bar
            targetSpriteBatch.Draw(Texture, Position, Color.White);

        }

        private Rectangle NegativeSpaceRectangle;
        private Rectangle PositiveSpaceRectangle;

        public Ship Ship { get; private set; }

        /// <summary>
        /// Current health of the bar (in %)
        /// </summary>
        public float CurrentHealth
        {
            get 
            {
                return currentHealth;
            }
            private set
            {
                NegativeSpaceRectangle = new Rectangle(
                    (int)(Texture.Width * value),
                    0,
                    (int)(Texture.Width * (1-value)),
                    Texture.Height);
                PositiveSpaceRectangle = new Rectangle(
                    0,
                    0,
                    (int)(Texture.Width * value),
                    Texture.Height);
                currentHealth = value;
            }
        }

        private float currentHealth ;
    }
}

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
        private static readonly Vector2 DISTANCE_FROM_SHIP = new Vector2(0, 14);

        private static readonly Color NEGATIVE_SPACE_COLOR = new Color(90, 90, 90);
        private static readonly Color HEALTH_BAR_COLOR_FULL = Color.DarkGreen;
        private static readonly Color HEALTH_BAR_COLOR_LOW = Color.DarkRed;

        public HealthBar(Ship entity) : base("HealthBar") 
        {
            Ship = entity;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Scale = 0.5f;
            Layer = Layers.HealthBar;

            Position = Ship.Position + DISTANCE_FROM_SHIP;
            CurrentHealth = Ship.GetHealthRatio();
        }

        public void Update(GameTime gameTime) 
        {            
            Position = Ship.Position + DISTANCE_FROM_SHIP;
            CurrentHealth = Ship.GetHealthRatio();
        }

        public override void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            // Draw the negative space for the health bar
            targetSpriteBatch.Draw(Texture, Position, NegativeSpaceRectangle, NEGATIVE_SPACE_COLOR, 0f, Origin, Scale, SpriteEffects.None, Layer);

            //Draw the current health level based on the current Health
            targetSpriteBatch.Draw(Texture, Position, PositiveSpaceRectangle, Color.Lerp(HEALTH_BAR_COLOR_LOW, HEALTH_BAR_COLOR_FULL, CurrentHealth), 
                0f, Origin, Scale, SpriteEffects.None, Layer - 0.01f);
        }

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
                    0,
                    0,
                    Texture.Width,
                    Texture.Height);
                PositiveSpaceRectangle = new Rectangle(
                    0,
                    0,
                    (int)(Texture.Width * value),
                    Texture.Height);
                currentHealth = value;
            }
        }

        private float currentHealth;

        private Rectangle NegativeSpaceRectangle;
        private Rectangle PositiveSpaceRectangle;

        public Ship Ship { get; private set; }
    }
}

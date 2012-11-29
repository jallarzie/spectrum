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
        private static readonly Vector2 DISTANCE_FROM_SHIP = new Vector2(0, 25);

        private static readonly Color NEGATIVE_SPACE_COLOR = Color.DimGray;
        private static readonly Color HEALTH_BAR_COLOR_NORMAL = Color.DarkRed;
        private static readonly Color HEALTH_BAR_COLOR_POISON = Color.DarkGreen;
        private static readonly Color HEALTH_BAR_COLOR_SLOW = Color.DarkBlue;
        private static readonly float FLASH_RATE = 4;

        private static SpriteFont SpriteFont = null;
            

        public HealthBar(Entity2D entity, string label) : base("HealthBar") 
        {
            this.label = label;

            if(SpriteFont == null)
                SpriteFont = Application.Instance.Content.Load<SpriteFont>("PlayerLabels");
                
            Entity = entity;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Scale = 0.5f;
            Layer = Layers.HealthBar;

            Position = Entity.Position + DISTANCE_FROM_SHIP;
            CurrentHealth = Entity.GetHealthRatio();

            currentColor = HEALTH_BAR_COLOR_NORMAL;
        }

        public void Update(GameTime gameTime) 
        {
            Position = Entity.Position + DISTANCE_FROM_SHIP;
            CurrentHealth = Entity.GetHealthRatio();

            if (label != "")
            {
                Vector2 labelSize = SpriteFont.MeasureString(label);
                Position = new Vector2(Position.X - labelSize.X / 2, Position.Y);

                labelPosition = Position;
                labelPosition.X += Origin.X * Scale;
                labelPosition.X += labelSize.X / 2;
                labelPosition.Y -= labelSize.Y / 2;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            // Draw the negative space for the health bar
            targetSpriteBatch.Draw(Texture, Position, NegativeSpaceRectangle, NEGATIVE_SPACE_COLOR, 0f, Origin, Scale, SpriteEffects.None, Layer);

            // Set color depending on Entity's status effects
            if (Entity.IsPoisoned && Entity.IsSlowed)
            {
                currentPoisonSlowFade += (float)gameTime.ElapsedGameTime.TotalSeconds * FLASH_RATE;
                currentColor = Color.Lerp(HEALTH_BAR_COLOR_POISON, HEALTH_BAR_COLOR_SLOW, (float)(Math.Cos(currentPoisonSlowFade) + 1) / 2);
            }
            else if (Entity.IsPoisoned)
            {
                currentColor = HEALTH_BAR_COLOR_POISON;
                currentPoisonSlowFade = 0f;
            }
            else if (Entity.IsSlowed)
            {
                currentColor = HEALTH_BAR_COLOR_SLOW;
                currentPoisonSlowFade = 1f;
            }
            else
            {
                currentColor = HEALTH_BAR_COLOR_NORMAL;
            }

            if (label != "")
            {
                targetSpriteBatch.DrawString(SpriteFont, label, labelPosition, Color.White, Rotation, Vector2.Zero, 1.0f, SpriteEffects.None, Layer);
            }

            //Draw the current health level based on the current Health
            targetSpriteBatch.Draw(Texture, Position, PositiveSpaceRectangle, currentColor, 0f, Origin, Scale, SpriteEffects.None, Layer - 0.01f);
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
                currentHealth = value;
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
            }
        }

        private float currentHealth;
        private float currentPoisonSlowFade;
        private Color currentColor;

        private string label;
        Vector2 labelPosition;

        private Rectangle NegativeSpaceRectangle;
        private Rectangle PositiveSpaceRectangle;

        public Entity2D Entity { get; private set; }
    }
}

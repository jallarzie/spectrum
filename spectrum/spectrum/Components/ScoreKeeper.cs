using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Spectrum.Components
{
    public delegate void OnThresholdReached();

    /// <summary>
    /// Class that keeps track of the current game's Score
    /// </summary>
    public class ScoreKeeper : Drawable
    {
        /// <summary>
        /// Score Value of a Power Core Hit
        /// </summary>
        public static readonly int POWERCORE_HIT_SCORE_VALUE = 5;
        
        /// <summary>
        /// Position of the Score String on the Field.
        /// </summary>
        private static readonly Vector2 POSITION = new Vector2(Application.Instance.GraphicsDevice.Viewport.Width * 0.1f,
                                                               Application.Instance.GraphicsDevice.Viewport.Height * 0.1f);
        private static readonly Vector2 CENTER = new Vector2(POSITION.X + 56, POSITION.Y + 18);

        private static readonly float OPACITY_DISTANCE = 300f;

        public int Value { get; set; }
        public int Level { get; private set; }
        public event OnThresholdReached OnThresholdReached;
        private int Threshold;
        private float Opacity;
        private List<Entity2D> Entities; // when close to this entity the opacity will lower

        private static SpriteFont SpriteFont;

        public ScoreKeeper(int level, List<Entity2D> entities, int threshold) 
        {
            SpriteFont = Application.Instance.Content.Load<SpriteFont>("ScoreFont");
            Level = level;
            Opacity = 1;
            Threshold = threshold;
            Entities = entities;
        }

        /// <summary>
        /// Add numOfPoints points to the CurrentScore.
        /// </summary>
        /// <param name="numOfPoints"></param>
        public void AddPoints(int numOfPoints) 
        {
            int oldValue = Value;
            Value += numOfPoints;
            if (OnThresholdReached != null && oldValue / Threshold < Value / Threshold)
                OnThresholdReached();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch) 
        {
            float leastDistance = 10000.0f;
            foreach (Entity2D entity in Entities)
            {
                float distance = Vector2.Distance(CENTER, entity.Position);
                if (distance < leastDistance)
                    leastDistance = distance;
            }
            if (leastDistance < OPACITY_DISTANCE)
                Opacity = leastDistance / OPACITY_DISTANCE;
            else
                Opacity = 1f;
            spriteBatch.DrawString(SpriteFont, "level " + Level, POSITION, Color.DimGray * Opacity, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, Layers.Hud);
            spriteBatch.DrawString(SpriteFont, string.Format("{0:D8}", Value), POSITION + new Vector2(0, 18), Color.White * Opacity, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, Layers.Hud);
        }
    }
}

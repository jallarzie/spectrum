using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spectrum.Library.Graphics;
using Microsoft.Xna.Framework;

namespace Spectrum.Components
{
    class Explosion : Sprite
    {
        private static int SRC_RECTANGLE_WIDTH = 70;
        private static int SRC_RECTANGLE_HEIGHT = 70;
        private static double FRAME_SWITCH_TIME_INTERVAL = 100;        
        private static int NUMBER_OF_FRAMES = 8;        

        /// <summary>
        /// Entity that is exploding.
        /// </summary>
        private Entity2D ExplodingEntity;

        /// <summary>
        /// The time at wich the Entity has been destroyed.
        /// </summary>
        private GameTime DestroyedGameTime;

        private int CurrentFrameIndex;

        public Explosion(Entity2D explodingEntity, GameTime destroyedGameTime)
            : base("smallExplosion")
        {
            DestroyedGameTime = destroyedGameTime;
            SourceRectangle = new Rectangle(0, 0, SRC_RECTANGLE_WIDTH, SRC_RECTANGLE_HEIGHT);
            CurrentFrameIndex = 0;

            ExplodingEntity = explodingEntity;
            Position = ExplodingEntity.Position;
            SetTint(ExplodingEntity.Tint);
        }

        public void Update(GameTime gameTime)
        {
            if (DestroyedGameTime.TotalGameTime.TotalMilliseconds + FRAME_SWITCH_TIME_INTERVAL * (CurrentFrameIndex)
                                < gameTime.TotalGameTime.TotalMilliseconds)
            {
                CurrentFrameIndex++;
                if (CurrentFrameIndex < NUMBER_OF_FRAMES)
                {
                    SourceRectangle = new Rectangle(
                        SourceRectangle.Value.Left + SRC_RECTANGLE_WIDTH,
                        SourceRectangle.Value.Top,
                        SRC_RECTANGLE_WIDTH,
                        SRC_RECTANGLE_HEIGHT
                        );
                }
                else
                {
                    SourceRectangle = new Rectangle(0, 0, 0, 0);
                }
            }

            base.Update();
        }
    }
}

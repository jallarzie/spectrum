using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace spectrum.GameComponents
{
    public class Ship : DrawableGameComponent
    {
        private static Rectangle SRC_RECTANGLE = new Rectangle(18, 22, 194, 91);

        private static Rectangle SRC_LEFTWING_RECTANGLE = new Rectangle(24, 252, 51, 54);
        private static Vector2 LEFTWING_OFFSET = new Vector2(-6, 37);

        private static Rectangle SRC_RIGHTWING_RECTANGLE = new Rectangle(24, 338, 51, 54);
        private static Vector2 RIGHTWING_OFFSET = new Vector2(154, 38);

        /// <summary>
        /// Top Left Position of the Ship
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Speed Of the Ship when it moves, in pixels/frame
        /// </summary>
        public int MovingSpeed;

        private Texture2D texture;

        private float Angle;

        public Ship(Game game, Vector2 position) : base(game) 
        {
            Position = position;
            MovingSpeed = 8;
            Angle = 0;
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("spriggan");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Left)) 
            {
                Position += MovingSpeed * new Vector2(-1, 0);
                Angle = 3 * (float)Math.PI/2;
            }
            if (keyboardState.IsKeyDown(Keys.Right)) 
            {
                Position += MovingSpeed * new Vector2(1, 0);
                Angle = (float)Math.PI/2;
            }
            if (keyboardState.IsKeyDown(Keys.Up)) 
            {
                Position += MovingSpeed * new Vector2(0, -1);
                Angle = 0;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                Position += MovingSpeed * new Vector2(0, 1);
                Angle = (float)Math.PI;
            }

            if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Up))
            {
                Angle = 11 * (float)Math.PI/6;
            }
            if (keyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyDown(Keys.Down))
            {
                Angle = 4 * (float)Math.PI / 3;
            }
            if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Up))
            {
                Angle = (float)Math.PI/6;
            }
            if (keyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyDown(Keys.Down))
            {
                Angle = 5 * (float)Math.PI / 6;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch batch = new SpriteBatch(Game.GraphicsDevice);

            batch.Begin();
            batch.Draw(texture, Position + new Vector2(SRC_RECTANGLE.Width / 2, SRC_RECTANGLE.Height / 2), SRC_RECTANGLE, Color.White, Angle, new Vector2(SRC_RECTANGLE.Width / 2, SRC_RECTANGLE.Height / 2), 1, SpriteEffects.None, 0);

            // Drawing the Ship's Wings
            batch.Draw(texture, Position + RIGHTWING_OFFSET, SRC_RIGHTWING_RECTANGLE, Color.White);
            batch.Draw(texture, Position + LEFTWING_OFFSET, SRC_LEFTWING_RECTANGLE, Color.White);
            batch.End();

            base.Draw(gameTime);
        }
    }
}

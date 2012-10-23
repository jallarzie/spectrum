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
        private static Rectangle SRC_RECTANGLE = new Rectangle(0, 0, 216, 97);

        /// <summary>
        /// Center Position of the Ship
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
            texture = Game.Content.Load<Texture2D>("ship");
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
            batch.Draw(
                texture: texture,
                position: Position, 
                sourceRectangle: SRC_RECTANGLE,
                color: Color.White, 
                rotation: Angle,
                origin: new Vector2(SRC_RECTANGLE.Width / 2, SRC_RECTANGLE.Height / 2),
                scale: 0.25f,
                effects: SpriteEffects.None,
                layerDepth: 0);
            
            batch.End();

            base.Draw(gameTime);
        }
    }
}

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
        /// Current Speed of the Ship, in pixels/frame
        /// </summary>
        public int Speed { get; private set; }

        /// <summary>
        /// Speed Of the Ship when it moves, in pixels/frame
        /// </summary>
        public int MovingSpeed;

        /// <summary>
        /// Unit Vector of the Direction of the Ship.
        /// </summary>
        public Vector2 Direction;


        private Texture2D texture;
        private KeyboardState previousKeyBoardState;

        public Ship(Game game, Vector2 position) : base(game) 
        {
            Position = position;
            Speed = 0;
            MovingSpeed = 8;
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
                Direction = new Vector2(-1, 0);
                Speed = MovingSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Right)) 
            { 
                Direction = new Vector2(1, 0);
                Speed = MovingSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Up)) 
            {
                Direction = new Vector2(0, -1);
                Speed = MovingSpeed;
            }
            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                Direction = new Vector2(0, 1);
                Speed = MovingSpeed;
            }
            else 
            {
                Speed = 0;
            }
            previousKeyBoardState = keyboardState;

            Position += Speed * Direction;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch batch = new SpriteBatch(Game.GraphicsDevice);

            batch.Begin();
            batch.Draw(texture, Position, SRC_RECTANGLE, Color.White);

            // Drawing the Ship's Wings
            batch.Draw(texture, Position + RIGHTWING_OFFSET, SRC_RIGHTWING_RECTANGLE, Color.White);
            batch.Draw(texture, Position + LEFTWING_OFFSET, SRC_LEFTWING_RECTANGLE, Color.White);
            batch.End();

            base.Draw(gameTime);
        }
    }
}

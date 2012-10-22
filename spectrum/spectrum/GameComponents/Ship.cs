using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace spectrum.GameComponents
{
    public class Ship : DrawableGameComponent
    {
        public Vector2 Position;

        private Texture2D texture;

        public Ship(Game game, Vector2 position) : base(game) 
        {
            Position = position;
        }

        protected override void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("spriggan");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch batch = new SpriteBatch(Game.GraphicsDevice);

            batch.Begin();
            batch.Draw(texture, Position, Color.White);
            batch.End();

            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;

namespace Spectrum.Components
{
    public class Ship : Sprite, PathAware
    {
        public Ship() : base("ship")
        {
            HealthBar = new HealthBar(this);
            CurrentHealthPoints = MaxHealthPoints = 200;
            Origin = new Vector2(Width / 2, Height / 2);
            BoundingArea = new Sphere(Position, 0.2f * (Height / 2), 0.2f * (Width / 2));
            Scale = 0.2f;
            Layer = Layers.Player;
            SetTint(Color.Black);
            PlayerIndex = PlayerIndex.One;
        }

        public void PathPosition(Vector2 position)
        {
            Position = position;
        }

        public void PathDirection(float angle)
        {
            Rotation = angle;
            BoundingArea.Shape.Rotation = angle;
        }

        public override void Draw(GameTime gameTime, SpriteBatch targetSpriteBatch)
        {
            base.Draw(gameTime, targetSpriteBatch);
            HealthBar.Draw(gameTime, targetSpriteBatch);
        }

        public override void ProcessHit(Laser laser)
        {
            Color oldTint = Tint;
            LoseTint(laser.Tint);
            if (oldTint == Tint)
            {
                base.ProcessHit(laser);
            }
        }

        public Path Path;
        public HealthBar HealthBar;
        public PlayerIndex PlayerIndex { get; protected set; }
    }
}

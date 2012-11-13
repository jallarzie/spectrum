using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spectrum.Library.Graphics;
using Spectrum.Library.Paths;
using Spectrum.Library.Collisions;

namespace Spectrum.Components
{
    public enum LaserAlignment { Player, Enemy }

    public class Laser : Sprite, PathAware
    {
        public Laser(Color tint, float charge, Vector2 position, Vector2 direction, float speed, LaserAlignment alignment) : base("laser")
        {
            Charge = MathHelper.Clamp(charge, 0f, 1f);

            Origin = new Vector2(Width / 2, Height / 2);
            Scale = 0.3f * (1 + Charge);
            Tint = tint;
            Position = position;
            Path = new Linear(this, direction);
            Alignment = alignment;
            Speed = speed;

            Damage = 10 + (int)(Charge * 40);
            Box = new Box(Position, Width * Scale, Height * Scale);
        }

        public void PathPosition(Vector2 position)
        {
            Position = position;
            this.Box.Rect.Center = Position;
        }

        public void PathDirection(float angle)
        {
            Rotation = angle;
            this.Box.Rect.Rotation = Rotation;
        }

        public bool IsVisible(Viewport viewport)
        {
            return (Position.X + Width / 2 >= viewport.X &&
                    Position.X - Width / 2 < viewport.Width &&
                    Position.Y + Height / 2 >= viewport.Y &&
                    Position.Y - Height / 2 < viewport.Height);
        }

        public Box GetBoundingBox() 
        {
            return Box;
        }

        public Path Path;
        public float Speed, Charge;
        public LaserAlignment Alignment;

        /// <summary>
        /// The amount of health point that will drop on the should the laser hit it.
        /// </summary>
        public int Damage;

        private Box Box;
    }
}

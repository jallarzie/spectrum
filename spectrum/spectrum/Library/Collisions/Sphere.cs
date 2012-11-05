using Shapes.Geometry;
using Microsoft.Xna.Framework;
using Shapes.Misc;

namespace Spectrum.Library.Collisions
{
    public class Sphere : Area
    {
        public Sphere(Vector2 center, float radius) 
        {
            Shape = new Ellipse(center, radius, radius); 
        }

        public bool CollidesWith(Area area)
        {
            return CollisionDetector2D.ShapeShapeIntersecting(this.GetShape(), area.GetShape());
        }

        public Shape GetShape()
        {
            return Shape;
        }

        private Shape Shape;
    }
}

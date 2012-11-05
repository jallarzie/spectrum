using Shapes.Geometry;
using Microsoft.Xna.Framework;
using Shapes.Misc;
namespace Spectrum.Library.Collisions
{
    public class Box : Area
    {
        public Box(Vector2 topLeft, float width, float height) 
        {
            Shape = new Rect(topLeft, width, height);
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

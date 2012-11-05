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

        public override bool CollidesWith(Area area)
        {
            return CollisionDetector2D.ShapeShapeIntersecting(this.Shape, area.Shape);
        }

        /// <summary>
        /// The Shape Object casted to a Rect Object
        /// </summary>
        public Rect Rect
        {
            get
            {
                return (Rect)Shape;
            }
            set
            {
                this.Shape = value;
            }
        }
    }
}

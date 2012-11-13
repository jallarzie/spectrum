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

        public Sphere(Vector2 center, float verticalRadius, float horizontalRadius)
        {
            Shape = new Ellipse(center, verticalRadius, horizontalRadius);
        }

        public override bool CollidesWith(Area area)
        {
            return CollisionDetector2D.ShapeShapeIntersecting(this.Shape, area.Shape);
        }

        /// <summary>
        /// The Shape Object casted to a Circle Object
        /// </summary>
        public Ellipse Circle 
        { 
            get 
            { 
                return (Ellipse)Shape;
            } 
            set 
            { 
                this.Shape = value;
            }
        }
    }
}

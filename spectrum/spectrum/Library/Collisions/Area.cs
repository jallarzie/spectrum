

using Shapes.Geometry;
namespace Spectrum.Library.Collisions
{
    public abstract class Area
    {
        /// <summary>
        /// Returns true if the passed area collides with this Area Object.
        /// </summary>
        /// <param name="area"></param>
        public abstract bool CollidesWith(Area area);

        /// <summary>
        /// The Area's corresponding Shape object.
        /// </summary>
        public Shape Shape;
    }
}

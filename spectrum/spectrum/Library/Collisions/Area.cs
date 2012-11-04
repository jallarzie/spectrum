

using Shapes.Geometry;
namespace Spectrum.Library.Collisions
{
    public interface Area
    {
        /// <summary>
        /// Returns true if the passed area collides with this Area Object.
        /// </summary>
        /// <param name="area"></param>
        bool CollidesWith(Area area);

        /// <summary>
        /// Returns the Area's corresponding Shape object.
        /// </summary>
        /// <returns></returns>
        Shape GetShape();
    }
}

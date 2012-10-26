using Microsoft.Xna.Framework;

namespace Spectrum.Library.Paths
{
    public interface PathAware
    {
        void PathPosition(Vector2 position);
        void PathDirection(float angle);
    }
}

using Microsoft.Xna.Framework;

namespace Spectrum.Library.Paths
{
    public interface Path
    {
        Vector2 Move(float distance);
        void Collide(Vector2 normal, Vector2 newPosition);
    }
}

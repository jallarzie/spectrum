using Microsoft.Xna.Framework;

namespace Spectrum.Library.Geometry
{
    public class Node : CoordinateSystem
    {
        public Node(CoordinateSystem parent = null)
        {
            Parent = parent;
            Position = new Vector2(0, 0);
            Rotation = 0f;
            Scale = 1f;
        }

        public CoordinateSystem Parent { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
    }
}

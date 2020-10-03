using Entygine.Mathematics;

namespace Entygine.Physics
{
    public class SphereCollider : Collider
    {
        public float Radius { get; set; }

        public override Vec3f FurthestPointInDirection(Vec3f dir)
        {
            return dir * Radius;
        }
    }
}

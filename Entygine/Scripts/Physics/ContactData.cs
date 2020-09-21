using Entygine.Mathematics;

namespace Entygine.Physics
{
    public struct ContactData
    {
        public Vec3f worldPointA;
        public Vec3f worldPointB;

        public Vec3f localPointA;
        public Vec3f localPointB;

        public Vec3f normal;
        public Vec3f tang1;
        public Vec3f tang2;

        public float depth;
    }
}

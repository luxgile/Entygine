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

        public ContactData(Vec3f worldPointA, Vec3f worldPointB, Vec3f localPointA, Vec3f localPointB, Vec3f normal, Vec3f tang1, Vec3f tang2, float depth)
        {
            this.worldPointA = worldPointA;
            this.worldPointB = worldPointB;
            this.localPointA = localPointA;
            this.localPointB = localPointB;
            this.normal = normal;
            this.tang1 = tang1;
            this.tang2 = tang2;
            this.depth = depth;
        }
    }
}

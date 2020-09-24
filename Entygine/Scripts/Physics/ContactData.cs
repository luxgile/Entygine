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

        public ContactData(Vec3f worldPointA, Vec3f localPointA, Vec3f worldPointB, Vec3f localPointB, Vec3f normal, float depth)
        {
            this.localPointA = localPointA;
            this.localPointB = localPointB;
            this.worldPointA = worldPointA;
            this.worldPointB = worldPointB;
            this.depth = depth;
            this.normal = normal;

            if (normal.x >= 0.57735f)
                tang1 = new Vec3f(normal.y, -normal.x, 0);
            else
                tang1 = new Vec3f(0.0f, normal.z, -normal.y);

            tang1.Normalize();
            tang2 = Vec3f.Cross(normal, tang1);
        }
    }
}

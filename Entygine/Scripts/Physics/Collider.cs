using BepuPhysics.Collidables;
using Entygine.Mathematics;

namespace Entygine.Physics
{
    public abstract class Collider
    {
        public PhysicBody body;
        public float mass;
        public Mat3f localInertiaTensor;
        public Vec3f localCentroid;

        public abstract Vec3f FurthestPointInDirection(Vec3f dir);
        public abstract IConvexShape Shape { get; }
    }
}

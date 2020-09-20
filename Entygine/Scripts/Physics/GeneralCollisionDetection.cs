using Entygine.Mathematics;

namespace Entygine.Physics
{
    public class GeneralCollisionDetection
    {
        private void CsoSupport(Collider a, Collider b, Vec3f dir, out Vec3f support, out Vec3f supportA, out Vec3f supportB)
        {
            PhysicBody bodyA = a.body;
            PhysicBody bodyB = b.body;

            Vec3f localDirA = bodyA.GlobalToLocalDir(dir);
            Vec3f localDirB = bodyB.GlobalToLocalDir(dir);

            supportA = a.FurthestPointInDirection(-localDirA);
            supportB = b.FurthestPointInDirection(-localDirB);

            support = supportA - supportB;
        }
    }
}

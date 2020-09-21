using Entygine.Mathematics;
using System.Collections.Generic;

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

        private bool Gjk(Collider colliderA, Collider colliderB)
        {
            List<Vec3f> simplex = new List<Vec3f>();
            Vec3f searchDir = Vec3f.Up;

            CsoSupport(colliderA, colliderB, searchDir, out Vec3f support, out _, out _);
            simplex.Add(support);

            while (true)
            {
                Vec3f closestPoint = GetClosestPointFromSimplex();
                if (closestPoint.Magnitude < MathUtils.Epsilon)
                    return true;

                while (simplex.Count > 3)
                    RemoveFurthestPoint(ref simplex);

                searchDir = -closestPoint.Normalized();
                CsoSupport(colliderA, colliderB, searchDir, out support, out _, out _);
                if (Vec3f.Dot(-closestPoint.Normalized(), (support - closestPoint).Normalized()) <= 0)
                    return false;

                simplex.Add(support);
            }

            Vec3f GetClosestPointFromSimplex()
            {
                switch (simplex.Count)
                {
                    case 1:
                        return simplex[0];
                    case 2:
                        return Line.ClosestPointToPoint(simplex[0], simplex[1], Vec3f.Zero);
                    case 3:
                        return Plane.ClosestPointToPoint((simplex[0] - simplex[1]).Normalized(), simplex[2], Vec3f.Zero);
                }

                throw new System.NotImplementedException();
            }

            static void RemoveFurthestPoint(ref List<Vec3f> points)
            {
                if (points.Count == 0)
                    throw new System.ArgumentException("Points need to have at least 1 point.");

                int index = -1;
                float d = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    float mag = points[i].SqrMagnitude;
                    if(mag > d)
                    {
                        d = mag;
                        index = i;
                    }
                }

                points.RemoveAt(index);
            }
        }
    }
}

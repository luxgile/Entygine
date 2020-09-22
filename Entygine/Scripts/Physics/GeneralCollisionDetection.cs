using Entygine.Mathematics;
using System;
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
            Vec3f localDirB = bodyB.GlobalToLocalDir(-dir);

            supportA = a.FurthestPointInDirection(localDirA);
            supportB = b.FurthestPointInDirection(localDirB);

            supportA = bodyA.LocalToGlobalPos(supportA);
            supportB = bodyB.LocalToGlobalPos(supportB);

            support = supportA - supportB;
        }

        private bool Gjk(Collider colliderA, Collider colliderB)
        {
            Vec3f a, b, c, d;
            a = b = c = d = Vec3f.Zero;
            Vec3f searchDir = colliderA.localCentroid - colliderB.localCentroid;

            CsoSupport(colliderB, colliderA, searchDir, out c, out _, out _);
            searchDir = -c;

            CsoSupport(colliderB, colliderA, searchDir, out b, out _, out _);

            if (Vec3f.Dot(b, searchDir) < 0)
                return false;

            Vec3f bc = c - b;
            searchDir = Vec3f.Cross(Vec3f.Cross(bc, -b), bc); //search perpendicular to line segment towards origin
            if (searchDir.SqrMagnitude < MathUtils.Epsilon)
            {
                searchDir = Vec3f.Cross(bc, Vec3f.Right); //normal with x-axis
                if (Vec3f.Aproximates(searchDir, Vec3f.Zero)) searchDir = Vec3f.Cross(bc, -Vec3f.Forward); //normal with z-axis
            }

            int simplexDimension = 2;
            for (int i = 0; i < 64; i++)
            {
                CsoSupport(colliderB, colliderA, searchDir, out a, out _, out _);
                if (Vec3f.Dot(a, searchDir) < 0) { return false; }

                simplexDimension++;
                if (simplexDimension == 3)
                {
                    UpdateSimplex3(ref a, ref b, ref c, ref d, ref simplexDimension, ref searchDir);
                }
                else if (UpdateSimplex4(ref a, ref b, ref c, ref d, ref simplexDimension, ref searchDir))
                {
                    //if (mtv) *mtv = EPA(a, b, c, d, coll1, coll2);
                    return true;
                }
            }
            return false;
        }

        private ContactData Epa(Collider colliderA, Collider colliderB, Vec3f a, Vec3f b, Vec3f c, Vec3f d)
        {
            List<Triangle> faces = new List<Triangle>
            {
                new Triangle(a, b, c),
                new Triangle(a, c, d),
                new Triangle(a, d, b),
                new Triangle(b, d, c)
            };

            Triangle prevFace = faces[0];
            for (int maxIt = 0; maxIt < 64; maxIt++)
            {
                int closestFace = GetClosestTriangle(faces, out float minDistance);

                if(/* IF CLOSEST IS NO CLOSER THAN PREVIOUS FACE */)
                {
                    //Project the origin onto the closest triangle. This is our closest point to the origin on the CSO’s boundary. 
                    //Compute the barycentric coordinates of this closest point using the vertices from this triangle. 
                    //The barycentric coordinates are linear combination coefficients of vertices from the triangle. For each collider, 
                    //linearly combine the support points corresponding to the vertices of the triangle, 
                    //using the same barycentric coordinates as coefficients. This gives us contact points on each collider in local space. 
                    //We can then convert these contact points to world space.
                }

                Vec3f searchDir = faces[closestFace].GetNormal();
                CsoSupport(colliderA, colliderB, searchDir, out Vec3f support, out _, out _);

                List<Line> edgesToFix = new List<Line>();
                for (int i = 0; i < faces.Count; i++)
                {
                    Triangle currFace = faces[i];

                    //Remove face and add edges be fixed. Remove if they are already in the list since it doesn't have any faces conected.
                    if (Vec3f.Dot(currFace.GetNormal(), support - currFace.a) > 0)
                    {
                        Line ab = currFace.AB;
                        if (edgesToFix.Contains(ab))
                            edgesToFix.Remove(ab);
                        else
                            edgesToFix.Add(ab);

                        Line ac = currFace.AC;
                        if (edgesToFix.Contains(ac))
                            edgesToFix.Remove(ac);
                        else
                            edgesToFix.Add(ac);

                        Line bc = currFace.BC;
                        if (edgesToFix.Contains(bc))
                            edgesToFix.Remove(bc);
                        else
                            edgesToFix.Add(bc);

                        faces.RemoveAt(i);
                        i--;
                    }
                }

                for (int i = 0; i < edgesToFix.Count; i++)
                {
                    Triangle face = new Triangle(edgesToFix[i].a, edgesToFix[i].b, support);
                    if (Vec3f.Dot(face.a, face.GetNormal()) + MathUtils.Epsilon < 0)
                    {
                        Vec3f temp = face.a;
                        face.a = face.b;
                        face.b = temp;
                    }

                    faces.Add(face);
                }
            }

            static int GetClosestTriangle(List<Triangle> faces, out float distance)
            {
                float mDist = distance = float.MaxValue;
                int index = -1;
                for (int i = 0; i < faces.Count; i++)
                {
                    float d = faces[i].ClosestPointToPoint(Vec3f.Zero).MagnitudeFast;
                    if(d < mDist)
                    {
                        mDist = d;
                        distance = d;
                        index = i;
                    }
                }
                return index;
            }
        }

        private bool UpdateSimplex4(ref Vec3f a, ref Vec3f b, ref Vec3f c, ref Vec3f d, ref int simplexDimension, ref Vec3f searchDir)
        {
            Vec3f ABC = Vec3f.Cross(b - a, c - a);
            Vec3f ACD = Vec3f.Cross(c - a, d - a);
            Vec3f ADB = Vec3f.Cross(d - a, b - a);

            Vec3f AO = -a;
            simplexDimension = 3;

            //Plane-test origin with 3 faces

            // Note: Kind of primitive approach used here; If origin is in front of a face, just use it as the new simplex.
            // We just go through the faces sequentially and exit at the first one which satisfies dot product. Not sure this 
            // is optimal or if edges should be considered as possible simplices? Thinking this through in my head I feel like 
            // this method is good enough. Makes no difference for AABBS, should test with more complex colliders.
            if (Vec3f.Dot(ABC, AO) > 0)
            {
                d = c;
                c = b;
                b = a;
                searchDir = ABC;
                return false;
            }

            if (Vec3f.Dot(ACD, AO) > 0)
            {
                b = a;
                searchDir = ACD;
                return false;
            }

            if (Vec3f.Dot(ADB, AO) > 0)
            {
                c = d;
                d = b;
                b = a;
                searchDir = ADB;
                return false;
            }

            return true;

            //Note: in the case where two of the faces have similar normals,
            //The origin could conceivably be closest to an edge on the tetrahedron
            //Right now I don't think it'll make a difference to limit our new simplices
            //to just one of the faces, maybe test it later.
        }

        private void UpdateSimplex3(ref Vec3f a, ref Vec3f b, ref Vec3f c, ref Vec3f d, ref int simplexDimension, ref Vec3f searchDir)
        {
            Vec3f normal = Vec3f.Cross(b - a, c - a);
            Vec3f originDir = -a;

            simplexDimension = 2;
            if (Vec3f.Dot(Vec3f.Cross(b - a, normal), originDir) > 0)
            {
                c = a;
                searchDir = Vec3f.Cross(Vec3f.Cross(b - a, originDir), b - a);
                return;
            }
            if (Vec3f.Dot(Vec3f.Cross(normal, c - a), originDir) > 0)
            {
                b = a;
                searchDir = Vec3f.Cross(Vec3f.Cross(c - a, originDir), c - a);
                return;
            }

            simplexDimension = 3;
            if (Vec3f.Dot(normal, originDir) > 0)
            {
                d = c;
                c = b;
                b = a;
                searchDir = normal;
                return;
            }

            d = b;
            b = a;
            searchDir = -normal;
            return;
        }
    }
}

using Entygine.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Physics
{
    public class GeneralCollisionDetection
    {
        private class FaceData
        {
            public Triangle triangle;
            public Triangle aSupport;
            public Triangle bSupport;
        }

        private class LineData
        {
            public Line line;
            public Line aSup;
            public Line bSup;

            public override bool Equals(object obj)
            {
                return obj is LineData data &&
                       line.Equals(data.line);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(line);
            }
        }

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

        private bool Gjk(Collider colliderA, Collider colliderB, out ContactData contactData)
        {
            contactData = default;
            Vec3f a, b, c, d;
            Vec3f Asa, Asb, Asc, Asd, Bsa, Bsb, Bsc, Bsd;
            a = b = c = d = Vec3f.Zero;
            Asa = Asb = Asc = Asd = Vec3f.Zero;
            Bsa = Bsb = Bsc = Bsd = Vec3f.Zero;
            Vec3f searchDir = colliderA.localCentroid - colliderB.localCentroid;

            CsoSupport(colliderB, colliderA, searchDir, out c, out Asc, out Bsc);
            searchDir = -c;

            CsoSupport(colliderB, colliderA, searchDir, out b, out Asb, out Bsb);

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
                CsoSupport(colliderB, colliderA, searchDir, out a, out Asa, out Bsa);
                if (Vec3f.Dot(a, searchDir) < 0) { return false; }

                simplexDimension++;
                if (simplexDimension == 3)
                {
                    UpdateSimplex3(ref a, ref b, ref c, ref d, ref simplexDimension, ref searchDir, ref Asa, ref Asb, ref Asc, ref Asd
                        , ref Bsa, ref Bsb, ref Bsc, ref Bsd);
                }
                else if (UpdateSimplex4(ref a, ref b, ref c, ref d, ref simplexDimension, ref searchDir, ref Asa, ref Asb, ref Asc, ref Asd
                        , ref Bsa, ref Bsb, ref Bsc, ref Bsd))
                {
                    contactData = Epa(colliderA, colliderB, a, b, c, d, Asa, Asb, Asc, Asd, Bsa, Bsb, Bsc, Bsd);
                    return true;
                }
            }
            return false;
        }

        private ContactData Epa(Collider colliderA, Collider colliderB, Vec3f a, Vec3f b, Vec3f c, Vec3f d,
            Vec3f Asa, Vec3f Asb, Vec3f Asc, Vec3f Asd, Vec3f Bsa, Vec3f Bsb, Vec3f Bsc, Vec3f Bsd)
        {
            List<FaceData> faces = new List<FaceData>
            {
                new FaceData{ triangle = new Triangle(a, b, c), aSupport = new Triangle(Asa, Asb, Asc), bSupport = new Triangle(Bsa, Bsb, Bsc) },
                new FaceData{ triangle = new Triangle(a, c, d), aSupport = new Triangle(Asa, Asc, Asd), bSupport = new Triangle(Bsa, Bsc, Bsd) },
                new FaceData{ triangle = new Triangle(a, d, b), aSupport = new Triangle(Asa, Asd, Asb), bSupport = new Triangle(Bsa, Bsd, Bsb) },
                new FaceData{ triangle = new Triangle(b, d, c), aSupport = new Triangle(Asb, Asd, Asc), bSupport = new Triangle(Bsb, Bsd, Bsc) },
            };

            FaceData prevFace = faces[0];
            for (int maxIt = 0; maxIt < 64; maxIt++)
            {
                int closestFaceIndex = GetClosestTriangle(faces, out float distance, out Vec3f projectedPoint);
                Vec3f searchDir = faces[closestFaceIndex].triangle.GetNormal();
                CsoSupport(colliderA, colliderB, searchDir, out Vec3f support, out Vec3f supportA, out Vec3f supportB);

                //If closest is no closer than previous face
                if (distance >= prevFace.triangle.ClosestPointToPoint(Vec3f.Zero).SqrMagnitude)
                {
                    FaceData closestFace = faces[closestFaceIndex];
                    Vec3f barycentric = closestFace.triangle.GetBarycentric(projectedPoint);
                    Vec3f localA = closestFace.aSupport.a * barycentric.x + closestFace.aSupport.b * barycentric.y + closestFace.aSupport.c * barycentric.z;
                    Vec3f localB = closestFace.bSupport.a * barycentric.x + closestFace.bSupport.b * barycentric.y + closestFace.bSupport.c * barycentric.z;
                    return new ContactData(colliderA.body.LocalToGlobalPos(localA), localA, colliderB.body.LocalToGlobalPos(localB)
                        , localB, searchDir, distance);
                }

                prevFace = faces[closestFaceIndex];


                List<LineData> edgesToFix = new List<LineData>();
                for (int i = 0; i < faces.Count; i++)
                {
                    FaceData currFace = faces[i];

                    //Remove face and add edges be fixed. Remove if they are already in the list since it doesn't have any faces conected.
                    if (Vec3f.Dot(currFace.triangle.GetNormal(), support - currFace.triangle.a) > 0)
                    {
                        LineData ab = new LineData() { line = currFace.triangle.AB, aSup = currFace.aSupport.AB, bSup = currFace.bSupport.AB };
                        if (edgesToFix.Contains(ab))
                            edgesToFix.Remove(ab);
                        else
                            edgesToFix.Add(ab);

                        LineData ac = new LineData() { line = currFace.triangle.AC, aSup = currFace.aSupport.AC, bSup = currFace.bSupport.AC };
                        if (edgesToFix.Contains(ac))
                            edgesToFix.Remove(ac);
                        else
                            edgesToFix.Add(ac);

                        LineData bc = new LineData() { line = currFace.triangle.BC, aSup = currFace.aSupport.BC, bSup = currFace.bSupport.BC };
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
                    LineData currEdge = edgesToFix[i];
                    FaceData face = new FaceData()
                    {
                        triangle = new Triangle(currEdge.line.a, currEdge.line.b, support),
                        aSupport = new Triangle(currEdge.aSup.a, currEdge.aSup.b, supportA),
                        bSupport = new Triangle(currEdge.bSup.a, currEdge.bSup.b, supportB),
                    };

                    if (Vec3f.Dot(face.triangle.a, face.triangle.GetNormal()) + MathUtils.Epsilon < 0)
                    {
                        Vec3f temp = face.triangle.a;
                        face.triangle.a = face.triangle.b;
                        face.triangle.b = temp;

                        temp = face.aSupport.a;
                        face.aSupport.a = face.aSupport.b;
                        face.aSupport.b = temp;

                        temp = face.bSupport.a;
                        face.bSupport.a = face.bSupport.b;
                        face.bSupport.b = temp;
                    }

                    faces.Add(face);
                }
            }

            return default;

            static int GetClosestTriangle(List<FaceData> faces, out float distance, out Vec3f projectedPoint)
            {
                projectedPoint = Vec3f.Zero;
                float mDist = distance = float.MaxValue;
                int index = -1;
                for (int i = 0; i < faces.Count; i++)
                {
                    Vec3f p = faces[i].triangle.ClosestPointToPoint(Vec3f.Zero);
                    float d = p.MagnitudeFast;
                    if (d < mDist)
                    {
                        projectedPoint = p;
                        mDist = d;
                        distance = d;
                        index = i;
                    }
                }
                return index;
            }
        }

        private bool UpdateSimplex4(ref Vec3f a, ref Vec3f b, ref Vec3f c, ref Vec3f d, ref int simplexDimension, ref Vec3f searchDir,
            ref Vec3f Asa, ref Vec3f Asb, ref Vec3f Asc, ref Vec3f Asd, ref Vec3f Bsa, ref Vec3f Bsb, ref Vec3f Bsc, ref Vec3f Bsd)
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
                Asd = Asc;
                Bsd = Bsc;

                c = b;
                Asc = Asb;
                Bsc = Bsb;

                b = a;
                Asb = Asa;
                Bsb = Bsa;

                searchDir = ABC;
                return false;
            }

            if (Vec3f.Dot(ACD, AO) > 0)
            {
                b = a;
                Asb = Asa;
                Bsb = Bsa;

                searchDir = ACD;
                return false;
            }

            if (Vec3f.Dot(ADB, AO) > 0)
            {
                c = d;
                Asc = Asd;
                Bsc = Bsd;

                d = b;
                Asd = Asb;
                Bsd = Bsb;

                b = a;
                Asb = Asa;
                Bsb = Bsa;

                searchDir = ADB;
                return false;
            }

            return true;

            //Note: in the case where two of the faces have similar normals,
            //The origin could conceivably be closest to an edge on the tetrahedron
            //Right now I don't think it'll make a difference to limit our new simplices
            //to just one of the faces, maybe test it later.
        }

        private void UpdateSimplex3(ref Vec3f a, ref Vec3f b, ref Vec3f c, ref Vec3f d, ref int simplexDimension, ref Vec3f searchDir,
            ref Vec3f Asa, ref Vec3f Asb, ref Vec3f Asc, ref Vec3f Asd, ref Vec3f Bsa, ref Vec3f Bsb, ref Vec3f Bsc, ref Vec3f Bsd)
        {
            Vec3f normal = Vec3f.Cross(b - a, c - a);
            Vec3f originDir = -a;

            simplexDimension = 2;
            if (Vec3f.Dot(Vec3f.Cross(b - a, normal), originDir) > 0)
            {
                c = a;
                Asc = Asa;
                Bsc = Bsa;

                searchDir = Vec3f.Cross(Vec3f.Cross(b - a, originDir), b - a);
                return;
            }
            if (Vec3f.Dot(Vec3f.Cross(normal, c - a), originDir) > 0)
            {
                b = a;
                Asb = Asa;
                Bsb = Bsa;

                searchDir = Vec3f.Cross(Vec3f.Cross(c - a, originDir), c - a);
                return;
            }

            simplexDimension = 3;
            if (Vec3f.Dot(normal, originDir) > 0)
            {
                d = c;
                Asd = Asc;
                Bsd = Bsc;

                c = b;
                Asc = Asb;
                Bsc = Bsb;

                b = a;
                Asb = Asa;
                Bsb = Bsa;

                searchDir = normal;
                return;
            }

            d = b;
            Asd = Asb;
            Bsd = Bsb;

            b = a;
            Asb = Asa;
            Bsb = Bsa;

            searchDir = -normal;
            return;
        }
    }
}

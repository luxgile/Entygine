using System;
using System.Diagnostics.CodeAnalysis;

namespace Entygine.Mathematics
{
    public struct Plane : IEquatable<Plane>
    {
        public float distance;
        public Vec3f normal;

        public Plane(Vec3f a, Vec3f b, Vec3f c)
        {
            normal = Vec3f.Cross(a - b, a - c).Normalized();
            distance = Vec3f.Dot(normal, a);
        }
        public Plane(Vec3f point, Vec3f normal)
        {
            this.normal = normal;
            distance = Vec3f.Dot(normal, point);
        }
        public Plane(Vec3f normal, float distance)
        {
            this.normal = normal;
            this.distance = distance;
        }

        public Vec3f ClosestPointToPoint(Vec3f point) => ClosestPointToPoint(distance, normal, point);
        public static Vec3f ClosestPointToPoint(float planeDistance, Vec3f planeNormal, Vec3f point)
        {
            float distance;
            Vec3f translationVector;

            distance = -SignedDistanceToPoint(planeDistance, planeNormal, point);

            translationVector = planeNormal.Normalized() * distance;

            return point + translationVector;
        }


        public float SignedDistanceToPoint(Vec3f point) => SignedDistanceToPoint(distance, normal, point);
        public static float SignedDistanceToPoint(float distance, Vec3f planeNormal, Vec3f point)
        {
            return Vec3f.Dot(planeNormal, point - (planeNormal * distance));
        }

        public override string ToString()
        {
            return $"n:{normal}, d:{distance}";
        }

        public bool Equals([AllowNull] Plane other)
        {
            return distance.Equals(other.distance) &&
                normal.Equals(other.normal);
        }
    }
}

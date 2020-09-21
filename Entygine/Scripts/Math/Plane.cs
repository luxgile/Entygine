using System;
using System.Diagnostics.CodeAnalysis;

namespace Entygine.Mathematics
{
    public struct Plane : IEquatable<Plane>
    {
        public Vec3f point;
        public Vec3f normal;

        public Plane(Vec3f point, Vec3f normal)
        {
            this.point = point;
            this.normal = normal;
        }

        public static Vec3f ClosestPointToPoint(Vec3f planePosition, Vec3f planeNormal, Vec3f point)
        {
            float distance;
            Vec3f translationVector;

            distance = -SignedDistanceToPoint(planeNormal, planePosition, point);

            translationVector = planeNormal.Normalized() * distance;

            return point + translationVector;
        }

        public static float SignedDistanceToPoint(Vec3f planePosition, Vec3f planeNormal, Vec3f point)
        {
            return Vec3f.Dot(planeNormal, (point - planePosition));
        }

        public override string ToString()
        {
            return $"p:{point}, n:{normal}";
        }

        public bool Equals([AllowNull] Plane other)
        {
            return point.Equals(other.point) &&
                normal.Equals(other.normal);
        }
    }
}

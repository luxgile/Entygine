using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Entygine.Mathematics
{
    public struct Line : IEquatable<Line>
    {
        public Vec3f a;
        public Vec3f b;

        public Line(Vec3f a, Vec3f b)
        {
            this.a = a;
            this.b = b;
        }

        public Vec3f ClosestPointToPoint(Vec3f point) => ClosestPointToPoint(a, b, point);

        public static Vec3f ClosestPointToPoint(Vec3f lineA, Vec3f lineB, Vec3f point)
        {
            Vec3f ab = lineB - lineA;
            Vec3f ap = point - lineA;

            float atbsqr = ab.SqrMagnitude;
            float dot = Vec3f.Dot(ab, ap);
            float dist = dot / atbsqr;

            if (dist < 0)
                return lineA;
            else if (dist > 1)
                return lineB;
            else
                return lineA + ab * dist;
        }

        public bool Equals([AllowNull] Line other)
        {
            return a.Equals(other.a) &&
                b.Equals(other.b);
        }

        public override string ToString()
        {
            return $"{a} <> {b}";
        }
    }
}

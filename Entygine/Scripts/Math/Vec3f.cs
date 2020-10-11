using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Entygine.Mathematics
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vec3f : IEquatable<Vec3f>
    {
        [FieldOffset(0)]
        public float x;
        [FieldOffset(4)]
        public float y;
        [FieldOffset(8)]
        public float z;

        public static readonly Vec3f Zero = new Vec3f(0, 0, 0);
        public static readonly Vec3f Up = new Vec3f(0, 1, 0);
        public static readonly Vec3f Right = new Vec3f(1, 0, 0);
        public static readonly Vec3f Forward = new Vec3f(0, 0, 1);

        public Vec3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float this[int index]
        {
            get { if (index == 0) return x; if (index == 1) return y; if (index == 2) return z; throw new IndexOutOfRangeException(); }
            set { if (index == 0) x = value; if (index == 1) y = value; if (index == 2) z = value; throw new IndexOutOfRangeException(); }
        }

        public static explicit operator OpenTK.Mathematics.Vector3(Vec3f v)
        {
            return new OpenTK.Mathematics.Vector3(v.x, v.y, v.z);
        }
        public static explicit operator Vec3f(OpenTK.Mathematics.Vector3 v)
        {
            return new Vec3f(v.X, v.Y, v.Z);
        }

        public static Vec3f operator +(Vec3f a, Vec3f b) => new Vec3f(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vec3f operator -(Vec3f a, Vec3f b) => new Vec3f(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vec3f operator -(Vec3f v) => new Vec3f(-v.x, -v.y, -v.z);
        public static Vec3f operator *(float v, Vec3f vec) => Multiply(vec, v);
        public static Vec3f operator *(Vec3f vec, float v) => Multiply(vec, v);

        public static Vec3f operator /(Vec3f vec, float v) => new Vec3f(vec.x / v, vec.y / v, vec.z / v);

        public static Vec3f Multiply(in Vec3f vec, in float v)
        {
            return new Vec3f(vec.x * v, vec.y * v, vec.z * v);
        }


        public static Vec3f operator *(Vec3f vec, Mat3f m) => Multiply(vec, m);
        /// <summary>
        /// Row multiplication between <see cref="Mat3f"/> amd <see cref="Vec3f"/>
        /// </summary>
        public static Vec3f Multiply(in Vec3f v, in Mat3f mat)
        {
            return new Vec3f(
                (v.x * mat.v00) + (v.y * mat.v01) + (v.z * mat.v02),
                (v.x * mat.v10) + (v.y * mat.v11) + (v.z * mat.v12),
                (v.x * mat.v20) + (v.y * mat.v21) + (v.z * mat.v22)
                );
        }

        public static Vec3f operator *(Mat3f m, Vec3f vec) => Multiply(m, vec);
        /// <summary>
        /// Column multiplication between <see cref="Mat3f"/> amd <see cref="Vec3f"/>
        /// </summary>
        public static Vec3f Multiply(in Mat3f mat, in Vec3f v)
        {
            return new Vec3f(
                (v.x * mat.v00) + (v.y * mat.v10) + (v.z * mat.v20),
                (v.x * mat.v01) + (v.y * mat.v11) + (v.z * mat.v21),
                (v.x * mat.v02) + (v.y * mat.v12) + (v.z * mat.v22)
                );
        }

        public static Vec3f Cross(in Vec3f lhs, in Vec3f rhs)
        {
            return new Vec3f()
            {
                x = (lhs.y * rhs.z) - (lhs.z * rhs.y),
                y = (lhs.z * rhs.x) - (lhs.x * rhs.z),
                z = (lhs.x * rhs.y) - (lhs.y * rhs.x),
            };
        }

        public static float Dot(in Vec3f lhs, in Vec3f rhs)
        {
            return (lhs.x * rhs.x) + (lhs.y * rhs.y) + (lhs.z * rhs.z);
        }

        public static float Distance(in Vec3f a, in Vec3f b)
        {
            return MathUtils.Sqrt(((b.x - a.x) * (b.x - a.x)) + ((b.y - a.y) * (b.y - a.y)) +
                                      ((b.z - a.z) * (b.z - a.z)));
        }

        /// <summary>
        /// Checks if two vectors are the same or really close to each other, avoiding float pointing errors.
        /// </summary>
        public static bool Aproximates(in Vec3f a, in Vec3f b)
        {
            return (a - b).SqrMagnitude < MathUtils.Epsilon;
        }

        public Vec3f Normalized()
        {
            Vec3f copy = this;
            copy.Normalize();
            return copy;
        }

        public void Normalize()
        {
            float scale = 1.0f / Magnitude;
            x *= scale;
            y *= scale;
            z *= scale;
        }

        public void NormalizeFast()
        {
            float scale = 1.0f / MagnitudeFast;
            x *= scale;
            y *= scale;
            z *= scale;
        }

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
        }

        public bool Equals([AllowNull] Vec3f other)
        {
            return Aproximates(this, other);
        }

        public float SqrMagnitude => (x * x) + (y * y) + (z * z);
        public float Magnitude => MathUtils.Sqrt(SqrMagnitude);
        public float MagnitudeFast => MathUtils.InverseSqrtFast(SqrMagnitude);

        public Vec2f XY => new Vec2f(x, y);

        public Vec2f XZ => new Vec2f(x, z);
        public Vec2f YZ => new Vec2f(y, z);
    }
}

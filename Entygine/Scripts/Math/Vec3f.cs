using System;
using System.Runtime.InteropServices;

namespace Entygine.Mathematics
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vec3f
    {
        [FieldOffset(0)]
        public float x;
        [FieldOffset(4)]
        public float y;
        [FieldOffset(8)]
        public float z;

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

        public static Vec3f operator +(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vec3f operator *(float v, Vec3f vec) => Multiply(vec, v);
        public static Vec3f operator *(Vec3f vec, float v) => Multiply(vec, v);

        public static Vec3f Multiply(in Vec3f vec, in float v)
        {
            return new Vec3f(vec.x * v, vec.y * v, vec.z * v);
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

        public Vec2f XY => new Vec2f(x, y);
        public Vec2f XZ => new Vec2f(x, z);
        public Vec2f YZ => new Vec2f(y, z);

        public static readonly Vec3f Zero = new Vec3f(0, 0, 0);
        public static readonly Vec3f Up = new Vec3f(0, 1, 0);
        public static readonly Vec3f Right = new Vec3f(1, 0, 0);
        public static readonly Vec3f Forward = new Vec3f(0, 0, 1);
    }
}

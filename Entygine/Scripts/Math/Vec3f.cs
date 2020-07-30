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

        public Vec2f XY => new Vec2f(x, y);
        public Vec2f XZ => new Vec2f(x, z);
        public Vec2f YZ => new Vec2f(y, z);

        public static readonly Vec3f Zero = new Vec3f(0, 0, 0);
        public static readonly Vec3f Up = new Vec3f(0, 1, 0);
        public static readonly Vec3f Right = new Vec3f(1, 0, 0);
        public static readonly Vec3f Forward = new Vec3f(0, 0, 1);
    }
}

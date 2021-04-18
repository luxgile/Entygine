using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Entygine.Mathematics
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vec2f
    {
        [FieldOffset(0)]
        public float x;
        [FieldOffset(4)]
        public float y;

        public Vec2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float this[int index]
        {
            get { if (index == 0) return x; if (index == 1) return y; throw new IndexOutOfRangeException(); }
            set { if (index == 0) x = value; if (index == 1) y = value; throw new IndexOutOfRangeException(); }
        }

        public static explicit operator Vector2(Vec2f v) => new Vector2(v.x, v.y);

        public Vec3f X0Y => new Vec3f(x, 0, y);

        public static readonly Vec2f Zero = new Vec2f(0, 0);
        public static readonly Vec2f Up = new Vec2f(0, 1);
        public static readonly Vec2f Right = new Vec2f(1, 0);
        public static readonly Vec2f One = new Vec2f(1, 1);
    }
}

using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Entygine.Mathematics
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Vec2i : IEquatable<Vec2i>
    {
        [FieldOffset(0)]
        public int x;
        [FieldOffset(4)]
        public int y;

        public Vec2i(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int this[int index]
        {
            get { if (index == 0) return x; if (index == 1) return y; throw new IndexOutOfRangeException(); }
            set { if (index == 0) x = value; if (index == 1) y = value; throw new IndexOutOfRangeException(); }
        }

        public static readonly Vec2i Zero = new Vec2i(0, 0);
        public static readonly Vec2i Up = new Vec2i(0, 1);
        public static readonly Vec2i Right = new Vec2i(1, 0);

        public static explicit operator Vec2i(Vector2 v) => new Vec2i((int)v.X, (int)v.Y);

        public static bool operator ==(Vec2i lhs, Vec2i rhs) => lhs.x == rhs.x && lhs.y == rhs.y;
        public static bool operator !=(Vec2i lhs, Vec2i rhs) => lhs.x != rhs.x || lhs.y != rhs.y;

        public bool Equals([AllowNull] Vec2i other)
        {
            return other.x == x && other.y == y;
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}

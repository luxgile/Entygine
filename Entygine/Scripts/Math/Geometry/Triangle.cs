namespace Entygine.Mathematics
{
    public struct Triangle
    {
        public Vec3f a, b, c;

        public Triangle(Vec3f a, Vec3f b, Vec3f c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public Line AB => new Line(a, b);
        public Line AC => new Line(a, b);
        public Line BC => new Line(c, b);
        public Vec3f GetNormal() => Vec3f.Cross(b - a, c - a).Normalized();
        public Plane CreatePlane() => new Plane(a, b, c);

        public static bool ContainsPoint(Vec3f a, Vec3f b, Vec3f c, Vec3f point)
        {
            a -= point;
            b -= point;
            c -= point;

            Vec3f u = Vec3f.Cross(b, c);
            Vec3f v = Vec3f.Cross(c, a);
            Vec3f w = Vec3f.Cross(a, b);

            return Vec3f.Dot(u, v) >= 0f && Vec3f.Dot(u, w) > 0f;
        }

        public Vec3f ClosestPointToPoint(Vec3f point) => ClosestPointToPoint(a, b, c, point);
        public static Vec3f ClosestPointToPoint(Vec3f a, Vec3f b, Vec3f c, Vec3f point)
        {
            Plane plane = new Plane(a, b, c);
            point = plane.ClosestPointToPoint(point);

            if (ContainsPoint(a, b, c, point))
                return point;

            Vec3f c1 = Line.ClosestPointToPoint(a, b, point);
            Vec3f c2 = Line.ClosestPointToPoint(b, c, point);
            Vec3f c3 = Line.ClosestPointToPoint(c, a, point);

            float mag1 = (point - c1).SqrMagnitude;
            float mag2 = (point - c2).SqrMagnitude;
            float mag3 = (point - c3).SqrMagnitude;

            MathUtils.Min(out int index, mag1, mag2, mag3);

            return index switch
            {
                0 => c1,
                1 => c2,
                2 => c3,
                _ => throw new System.ArithmeticException(),
            };
        }
    }
}

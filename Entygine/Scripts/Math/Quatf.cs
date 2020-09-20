namespace Entygine.Mathematics
{
    public struct Quatf
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Quatf(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Quatf(Vec3f xyz, float w)
        {
            this.x = xyz.x;
            this.y = xyz.y;
            this.z = xyz.z;
            this.w = w;
        }

        public static Quatf operator *(Quatf lhs, Quatf rhs)
        {
            Vec3f lxyz = lhs.XYZ;
            Vec3f rxyz = rhs.XYZ;
            return new Quatf(
                (rhs.w * lxyz) + (lhs.w * rxyz) + Vec3f.Cross(lxyz, rxyz),
                (lhs.w * rhs.w) - Vec3f.Dot(lxyz, rxyz));
        }
        public static Quatf operator *(Quatf lhs, float rhs)
        {
            return new Quatf(lhs.XYZ * rhs, lhs.w * rhs);
        }

        public void GetAngleAxis(out Vec3f axis, out float angle)
        {
            Quatf q = this;
            if (MathUtils.Absolute(q.w) > 1.0f)
            {
                q.Normalize();
            }

            angle = 2.0f * (float)MathUtils.Acos(q.w);

            var den = (float)MathUtils.Sqrt(1.0f - (q.w * q.w));
            if (den > 0.0001f)
            {
                axis = q.XYZ / den;
            }
            else
            {
                // This occurs when the angle is zero.
                // Not a problem: just set an arbitrary normalized axis.
                axis = Vec3f.Up;
            }
        }

        public Quatf Normalized()
        {
            Quatf copy = this;
            copy.Normalized();
            return copy;
        }
        public void Normalize()
        {
            float scale = 1 / Magnitude;
            x *= scale;
            y *= scale;
            z *= scale;
            w *= scale;
        }

        public float SqrMagnitude => (x * x) + (y * y) + (z * z) + (w * w);
        public float Magnitude => MathUtils.Sqrt(SqrMagnitude);
        public float MagnitudeFast => MathUtils.InverseSqrtFast(SqrMagnitude);

        public Vec3f XYZ => new Vec3f(x, y, z);
    }
}

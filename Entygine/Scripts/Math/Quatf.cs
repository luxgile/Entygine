namespace Entygine.Mathematics
{
    public struct Quatf
    {
        private Vec3f xyz;
        private float w;

        public Quatf(float x, float y, float z, float w)
        {
            xyz = new Vec3f(x, y, z);
            this.w = w;
        }

        public Quatf(Vec3f xyz, float w)
        {
            this.xyz = xyz;
            this.w = w;
        }

        public static Quatf operator *(Quatf lhs, Quatf rhs)
        {
            return new Quatf(
                (rhs.w * lhs.xyz) + (lhs.w * rhs.xyz) + Vec3f.Cross(lhs.xyz, rhs.xyz),
                (lhs.w * rhs.w) - Vec3f.Dot(lhs.xyz, rhs.xyz));
        }
        public static Quatf operator *(Quatf lhs, float rhs)
        {
            return new Quatf(lhs.xyz * rhs, lhs.w * rhs);
        }
    }
}

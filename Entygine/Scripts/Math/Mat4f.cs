namespace Entygine.Mathematics
{
    public struct Mat4f
    {
        public float v00;
        public float v10;
        public float v20;
        public float v30;
        public float v01;
        public float v11;
        public float v21;
        public float v31;
        public float v02;
        public float v12;
        public float v22;
        public float v32;
        public float v03;
        public float v13;
        public float v23;
        public float v33;

        public Vec4f Row1 => new Vec4f(v00, v10, v20, v30);
        public Vec4f Row2 => new Vec4f(v01, v11, v21, v31);
        public Vec4f Row3 => new Vec4f(v02, v12, v22, v32);
        public Vec4f Row4 => new Vec4f(v03, v13, v23, v33);

        public Vec4f Col1 => new Vec4f(v00, v01, v02, v03);
        public Vec4f Col2 => new Vec4f(v10, v11, v12, v13);
        public Vec4f Col3 => new Vec4f(v20, v21, v22, v23);
        public Vec4f Col4 => new Vec4f(v30, v31, v32, v33);
    }
}

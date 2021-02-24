using OpenTK.Mathematics;

namespace Entygine.Rendering
{
    public struct Color01
    {
        public float r;
        public float g;
        public float b;
        public float a;

        public Color01(float r, float g, float b, float a)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;
        }

        public static explicit operator Color4(Color01 color)
        {
            return new Color4(color.r, color.g, color.b, color.a);
        }

        public static readonly Color01 white = new Color01(1, 1, 1, 1);
    }
}

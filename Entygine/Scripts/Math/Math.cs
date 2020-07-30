using OpenToolkit.Mathematics;

namespace Entygine.Mathematics
{
    public static partial class Math
    {
        public static readonly float Epsilon = 0.0000001f;

        public static float Absolute(float v) => MathHelper.Abs(v);
        public static bool IsZero(float v) => Absolute(v) < Epsilon;
        public static float Round(float v) => (float)MathHelper.Round(v);
        public static float Ceil(float v) => (float)MathHelper.Ceiling(v);
        public static float Floor(float v) => (float)MathHelper.Floor(v);
        public static float Clamp(float value, float min, float max) => MathHelper.Clamp(value, min, max);
    }
}

using OpenToolkit.Mathematics;
using System;

namespace Entygine.Mathematics
{
    public static partial class MathUtils
    {
        public static readonly float Epsilon = 0.0000001f;

        public static float Absolute(float v) => MathHelper.Abs(v);
        public static bool IsZero(float v) => Absolute(v) < Epsilon;
        public static float Round(float v) => (float)MathHelper.Round(v);
        public static float Ceil(float v) => (float)MathHelper.Ceiling(v);
        public static float Floor(float v) => (float)MathHelper.Floor(v);
        public static float Clamp(float value, float min, float max) => MathHelper.Clamp(value, min, max);
        public static float Sqrt(float v) => (float)MathHelper.Sqrt(v);
        public static float InverseSqrtFast(float v) => (float)MathHelper.InverseSqrtFast(v);
        public static float Cos(float radians) => (float)MathHelper.Cos(radians);
        public static float Acos(float radians) => (float)MathHelper.Acos(radians);
        public static float Sin(float radians) => (float)MathHelper.Sin(radians);
        public static float Asin(float radians) => (float)MathHelper.Asin(radians);
    }
}

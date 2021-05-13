using OpenTK.Mathematics;
using System;

namespace Entygine.Mathematics
{
    public static class MathUtils
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

        public static uint Max(uint v1, uint v2) => v1 > v2 ? v1 : v2;
        public static int Max(int v1, int v2) => v1 > v2 ? v1 : v2;
        public static float Max(float v1, float v2) => v1 > v2 ? v1 : v2;
        public static float Max(params float[] values)
        {
            int length = values.Length;
            if (length == 0)
                return 0.0f;

            float num = values[0];
            for (int i = 1; i < length; ++i)
            {
                if (values[i] > num)
                    num = values[i];
            }
            return num;
        }
        public static float Max(out int index, params float[] values)
        {
            index = -1;
            int length = values.Length;
            if (length == 0)
                return 0.0f;

            float num = values[0];
            index = 0;
            for (int i = 1; i < length; ++i)
            {
                if (values[i] > num)
                {
                    index = i;
                    num = values[i];
                }
            }
            return num;
        }

        public static uint Min(uint v1, uint v2) => v1 < v2 ? v1 : v2;
        public static int Min(int v1, int v2) => v1 < v2 ? v1 : v2;
        public static float Min(float v1, float v2) => v1 < v2 ? v1 : v2;
        public static float Min(params float[] values)
        {
            int length = values.Length;
            if (length == 0)
                return 0.0f;

            float num = values[0];
            for (int i = 1; i < length; ++i)
            {
                if (values[i] < num)
                    num = values[i];
            }
            return num;
        }

        public static float Min(out int index, params float[] values)
        {
            index = -1;
            int length = values.Length;
            if (length == 0)
                return 0.0f;

            float num = values[0];
            index = 0;
            for (int i = 1; i < length; ++i)
            {
                if (values[i] < num)
                {
                    index = i;
                    num = values[i];
                }
            }
            return num;
        }
    }
}

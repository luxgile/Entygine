using Entygine.Mathematics;
using System;

namespace Entygine
{
    public static class AppScreen
    {
        private static int width;
        private static float aspect;

        public static Vec2i Resolution
        {
            get => new Vec2i(width, (int)(width * (1 / aspect)));
            set
            {
                width = value.x;
                aspect = (float)value.x / (float)value.y;

                ResolutionChanged?.Invoke(Resolution);
            }
        }

        public static float Aspect => aspect;

        public static event Action<Vec2i> ResolutionChanged;
    }
}

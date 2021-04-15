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
            get => new Vec2i(width, (int)(width * aspect));
            set
            {
                width = value.x;
                aspect = value.x / value.y;

                ResolutionChanged?.Invoke(Resolution);
            }
        }

        public static float Aspect => aspect;

        public static event Action<Vec2i> ResolutionChanged;
    }
}

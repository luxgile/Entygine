using Entygine.Mathematics;
using Entygine.Rendering.Pipeline;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Entygine.DevTools
{
    public static class DevGizmos
    {
        private static ConcurrentBag<GizmoPoint> points = new();
        private static ConcurrentBag<GizmoLine> lines = new();

        public static void DrawPoint(Vec3f point)
        {
            points.Add(new GizmoPoint(point));
        }

        public static void DrawLine(Line line) => DrawLine(line.a, line.b);
        public static void DrawLine(Vec3f a, Vec3f b)
        {
            lines.Add(new GizmoLine(a, b));
        }

        public static void Dispatch()
        {
            if (RenderPipelineCore.TryGetContext(out GizmosContextData gizmosContextData))
            {
                foreach (GizmoLine line in lines)
                    gizmosContextData.LinesOrder.AddOrder(line);
                lines.Clear();

                foreach (GizmoPoint point in points)
                    gizmosContextData.PointsOrder.AddOrder(point);
                points.Clear();
            }
        }
    }
}

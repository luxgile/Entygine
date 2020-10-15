using Entygine.Mathematics;
using Entygine.Rendering.Pipeline;

namespace Entygine.DevTools
{
    public class DevGizmos
    {
        public static void DrawPoint(Vec3f point)
        {
            if (RenderPipelineCore.TryGetContext(out GizmosContextData gizmosContextData))
                gizmosContextData.PointsOrder.AddOrder(new GizmoPoint(point));
        }

        public static void DrawLine(Line line) => DrawLine(line.a, line.b);
        public static void DrawLine(Vec3f a, Vec3f b)
        {
            if (RenderPipelineCore.TryGetContext(out GizmosContextData gizmosContextData))
                gizmosContextData.LinesOrder.AddOrder(new GizmoLine(a, b));
        }
    }
}

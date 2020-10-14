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
    }
}

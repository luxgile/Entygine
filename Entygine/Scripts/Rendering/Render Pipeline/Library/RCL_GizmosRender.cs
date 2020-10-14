using OpenToolkit.Mathematics;

namespace Entygine.Rendering.Pipeline
{
    public static partial class RenderCommandsLibrary
    {
        public static RenderCommand DrawGizmos(CameraData camera, Matrix4 transform)
        {
            return new RenderCommand("Draw Gizmos", (ref RenderContext context) =>
            {
                if (!context.TryGetData(out GizmosContextData gizmosContextData))
                    return;

                gizmosContextData.GizmoMaterial.SetMatrix("view", transform);
                gizmosContextData.GizmoMaterial.SetMatrix("projection", camera.CalculateProjection());
                gizmosContextData.GizmoMaterial.UseMaterial();

                gizmosContextData.PointsOrder.PerformDraw();
            });
        }
    }
}

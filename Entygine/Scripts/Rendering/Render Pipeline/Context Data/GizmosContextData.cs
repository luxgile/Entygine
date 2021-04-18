using Entygine.DevTools;

namespace Entygine.Rendering.Pipeline
{
    public class GizmosContextData : RenderContextData
    {
        public Material GizmoMaterial { get; set; }
        public GizmoPointOrder PointsOrder { get; }
        public GizmoLineOrder LinesOrder { get; }

        public GizmosContextData()
        {
            PointsOrder = new GizmoPointOrder();
            LinesOrder = new GizmoLineOrder();
            Shader gizmoShader = Shader.CreateShaderWithPath(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\gizmo.vert"), AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\gizmo.frag"), "Gizmos");
            GizmoMaterial = new Material(gizmoShader, null);
            GizmoMaterial.LoadMaterial();
        }

        public void Clear()
        {
            PointsOrder.Clear();
            LinesOrder.Clear();
        }
    }
}

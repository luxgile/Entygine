using Entygine.DevTools;

namespace Entygine.Rendering.Pipeline
{
    public class GizmosContextData : RenderContextData
    {
        public Material GizmoMaterial { get; set; }
        public GizmoPointOrder PointsOrder { get; }

        public GizmosContextData()
        {
            PointsOrder = new GizmoPointOrder();
            Shader gizmoShader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\gizmo.vert"), AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\gizmo.frag"));
            GizmoMaterial = new Material(gizmoShader, null);
            GizmoMaterial.LoadMaterial();
        }

        public void Clear()
        {
            PointsOrder.Clear();
        }
    }
}

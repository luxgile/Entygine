using Entygine.Rendering;

namespace Entygine.UI
{
    public class UIImage : UIElement, UI_IRenderable, IRaycastable
    {
        private Color01 color;

        public UIImage()
        {
            Shader shader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.vert")
                , AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.frag"));
            Material = new Material(shader, Texture2D.CreateWhiteTexture(1, 1));
            Material.LoadMaterial();
            color = new Color01(1, 1, 1, 0.1f);
        }

        public bool Raycast(MouseData mouse)
        {
            return Rect.Contains(mouse);
        }

        public void DrawUI(Mesh mesh)
        {
            OpenTK.Mathematics.Matrix4 modelMatrix = Rect.GetModelMatrix();
            Material.SetMatrix("model", modelMatrix);
            Material.SetColor("color", color);

            GraphicsAPI.UseMeshMaterial(mesh, Material);

            GraphicsAPI.DrawTriangles(mesh.GetIndiceCount());
        }

        public Rect Rect { get; set; }
        public Material Material { get; set; }
    }
}

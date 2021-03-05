using Entygine.DevTools;
using Entygine.Rendering;

namespace Entygine.UI
{
    public class UIImage : UIElement, UI_IRenderable, IRaycastable
    {
        public bool HasRaycasting { get; set; }

        public UIImage()
        {
            Shader shader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.vert")
                , AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.frag"));
            Material = new Material(shader, Texture2D.CreateWhiteTexture(1, 1));
            Material.LoadMaterial();
        }

        public bool Raycast(MouseData mouse)
        {
            if (!HasRaycasting)
                return false;

            return Rect.Contains(mouse.position);
        }

        public void DrawUI(Mesh mesh)
        {
            OpenTK.Mathematics.Matrix4 modelMatrix = Rect.GetModelMatrix();
            Material.SetMatrix("model", modelMatrix);
            Material.SetColor("color", Color);

            GraphicsAPI.UseMeshMaterial(mesh, Material);

            GraphicsAPI.DrawTriangles(mesh.GetIndiceCount());
        }

        public Color01 Color { get; set; }
        public Rect Rect { get; set; }
        public Material Material { get; set; }
    }
}

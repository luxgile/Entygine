using Entygine.Rendering;

namespace Entygine.UI
{
    public class UIImage : UIElement, UI_IRenderable, IRaycasteable
    {
        public UIImage()
        {
            Shader shader = new Shader(AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.vert")
                , AssetBrowser.Utilities.LocalToAbsolutePath(@"Shaders\uiStandard.frag"));
            Material = new Material(shader, Texture2D.CreateWhiteTexture(1, 1));
            Material.LoadMaterial();
            Color = new Color01(1, 1, 1, 1);
        }

        public bool Raycast(MouseData mouse)
        {
            return Rect.Contains(mouse);
        }

        public Rect Rect { get; set; }
        public Material Material { get; set; }
        public Color01 Color { get; set; }
    }
}

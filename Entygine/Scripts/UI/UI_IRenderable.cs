using Entygine.Rendering;

namespace Entygine.UI
{
    public interface UI_IRenderable
    {
        Material Material { get; set; }
        Color01 Color { get; set; }
        Rect Rect { get; set; }

        void DrawUI(Mesh mesh);
    }
}

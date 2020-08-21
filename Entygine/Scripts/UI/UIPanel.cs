using OpenToolkit.Mathematics;

namespace Entygine.UI
{
    public abstract class UIPanel : UIElement
    {
        public abstract Rect[] GetChildsRect(Rect parentRect);
    }
}

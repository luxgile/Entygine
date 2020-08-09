using OpenToolkit.Mathematics;

namespace Entygine.UI
{
    public class UICanvas
    {
        public UIElement Root { get; set; }

        public UICanvas()
        {
            Root = new UIElement();
        }

        public void PerformLogic()
        {
            Root.PerformLogic();
        }
    }
}

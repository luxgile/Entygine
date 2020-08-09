using System.Collections.Generic;

namespace Entygine.UI
{
    public class UIElement
    {
        private IStyle style = new UIStyle();
        private List<UIElement> childs = new List<UIElement>();

        public void PerformLogic()
        {
            OnPerformLogic();

            for (int i = 0; i < childs.Count; i++)
                childs[i].PerformLogic();
        }

        protected virtual void OnPerformLogic() { }

        public IStyle Style => style;
    }
}

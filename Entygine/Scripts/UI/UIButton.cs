using Entygine.Rendering;
using System;

namespace Entygine.UI
{
    public class UIButton : UIElement, IRaycastable, IMouseEnter, IMouseExit, IMouseClick
    {
        public event Action OnClick;
        public UI_IRenderable Renderable { get; set; }

        private Rect cachedRect;

        public UIButton()
        {
            UIImage buttonImg = new UIImage() { Color = Color01.white };
            Renderable = buttonImg;
            Children.Add(buttonImg);
        }

        public override Rect GetRect(Rect parentRect)
        {
            cachedRect = base.GetRect(parentRect);
            return cachedRect;
        }

        public void OnMouseClick(MouseData mouseData)
        {
            OnClick?.Invoke();
        }

        public void OnMouseEnter(MouseData mouseData)
        {
            if (Renderable != null)
                Renderable.Color = Color01.green;
        }

        public void OnMouseExit(MouseData mouseData)
        {
            if (Renderable != null)
                Renderable.Color = Color01.white;
        }

        public bool Raycast(MouseData mouse)
        {
            return cachedRect.Contains(mouse.position);
        }
    }
}

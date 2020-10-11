using Entygine.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;

namespace Entygine.UI
{
    public class UIElement
    {
        private IStyle style = new UIStyle();
        private List<UIElement> children = new List<UIElement>();

        public virtual Rect GetRect(Rect parentRect)
        {
            Vector2 pos = parentRect.pos;
            Vector2 size = parentRect.size;
            pos.X += Style.leftPadding;
            pos.Y += Style.bottomPadding;
            size.X -= Style.rightPadding * 2;
            size.Y -= Style.topPadding * 2;
            return new Rect(pos, size);
        }

        public IStyle Style => style;
        public List<UIElement> Children => children;
    }
}

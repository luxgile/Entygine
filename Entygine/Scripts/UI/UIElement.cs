using Entygine.Rendering;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;

namespace Entygine.UI
{
    public class UIElement
    {
        public Padding Padding { get; set; }
        private List<UIElement> children = new List<UIElement>();

        public virtual Rect GetRect(Rect parentRect)
        {

            return Padding.SolveRect(parentRect);
        }

        public List<UIElement> Children => children;
    }
}

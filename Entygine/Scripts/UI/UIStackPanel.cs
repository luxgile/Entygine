using OpenToolkit.Mathematics;

namespace Entygine.UI
{
    public class UIStackPanel : UIPanel
    {
        public override Rect[] GetChildsRect(Rect parentModel)
        {
            Rect[] rects = new Rect[Children.Count];
            Vector2 parentPos = parentModel.pos;
            Vector2 parentSize = parentModel.size;
            float sizeDelta = parentSize.Y / Children.Count;
            Vector2 currentPos = parentPos;
            for (int i = 0; i < Children.Count; i++)
            {
                Vector2 childSize = new Vector2(parentSize.X, sizeDelta);
                rects[i] = new Rect(currentPos, childSize);
                currentPos.Y += sizeDelta;
            }
            return rects;
        }
    }
}

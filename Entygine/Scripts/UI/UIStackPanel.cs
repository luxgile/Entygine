using OpenTK.Mathematics;

namespace Entygine.UI
{
    public class UIStackPanel : UIPanel
    {
        public enum EOrientation : byte { Vertical, Horizontal }
        public EOrientation Orientation { get; set; } = EOrientation.Vertical;

        public override Rect[] GetChildsRect(Rect parentModel)
        {
            Rect[] rects = new Rect[Children.Count];
            Vector2 parentPos = parentModel.pos;
            Vector2 parentSize = parentModel.size;
            Vector2 currentPos = parentPos;

            if (Orientation == EOrientation.Vertical)
            {
                float sizeDelta = parentSize.Y / Children.Count;
                for (int i = 0; i < Children.Count; i++)
                {
                    Vector2 childSize = new Vector2(parentSize.X, sizeDelta);
                    rects[i] = new Rect(currentPos, childSize);
                    currentPos.Y += sizeDelta;
                }
            }
            else
            {
                float sizeDelta = parentSize.X / Children.Count;
                for (int i = 0; i < Children.Count; i++)
                {
                    Vector2 childSize = new Vector2(sizeDelta, parentSize.Y);
                    rects[i] = new Rect(currentPos, childSize);
                    currentPos.X += sizeDelta;
                }
            }

            return rects;
        }
    }
}

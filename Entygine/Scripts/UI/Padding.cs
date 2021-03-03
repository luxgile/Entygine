using OpenTK.Mathematics;

namespace Entygine.UI
{
    public struct Padding
    {
        public enum EMode { Pixel, Relative }
        public struct PaddingElement 
        { 
            public EMode mode; 
            public int pxPad; 
            public float relPad; 

            public PaddingElement(int px) { mode = EMode.Pixel; pxPad = px; relPad = 0; }
            public PaddingElement(float rel) { mode = EMode.Relative; pxPad = 0; relPad = rel; }
        }

        public Rect SolveRect(Rect parentRect)
        {
            Vector2 pos = parentRect.pos;
            Vector2 size = parentRect.size;
            int pxLeft = left.mode == EMode.Pixel ? left.pxPad : (int)(left.relPad * parentRect.size.X);
            int pxBottom = bottom.mode == EMode.Pixel ? bottom.pxPad : (int)(bottom.relPad * parentRect.size.Y);
            int pxRight = right.mode == EMode.Pixel ? right.pxPad : (int)(right.relPad * parentRect.size.X);
            int pxTop = top.mode == EMode.Pixel ? top.pxPad : (int)(top.relPad * parentRect.size.Y);

            pos.X += pxLeft;
            pos.Y += pxBottom;
            size.X -= pxRight + pxLeft;
            size.Y -= pxTop + pxBottom;

            return new Rect(pos, size);
        }

        public PaddingElement top;
        public PaddingElement bottom;
        public PaddingElement left;
        public PaddingElement right;
    }
}

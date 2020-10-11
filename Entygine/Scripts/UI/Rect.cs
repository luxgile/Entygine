using OpenTK.Mathematics;
using System;

namespace Entygine.UI
{
    public struct Rect
    {
        public Vector2 pos;
        public Vector2 size;

        public Rect(Vector2 pos, Vector2 size)
        {
            this.pos = pos;
            this.size = size;
        }

        public static Rect operator *(Rect r, float scale) => Scale(r, scale);
        public static Rect Scale(in Rect r, in float scale)
        {
            Rect result = r;
            result.pos *= scale;
            result.size *= scale;
            return result;
        }

        public Matrix4 GetModelMatrix()
        {
            Matrix4 model = Matrix4.Identity;
            model *= Matrix4.CreateScale(new Vector3(size));
            model *= Matrix4.CreateTranslation(new Vector3(pos));
            return model;
        }

        public bool Contains(MouseData mouse)
        {
            Vector2 mousePos = mouse.position;
            return mousePos.X >= pos.X && mousePos.X <= pos.X + size.X
                && mousePos.Y >= pos.Y && mousePos.Y <= pos.Y + size.Y;
        }
    }
}

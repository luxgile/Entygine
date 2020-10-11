using OpenTK.Graphics.OpenGL4;
using System;

namespace Entygine.Rendering
{
    public enum VertexAttributeFormat
    {
        Float32, Int32,
    }

    public static class VertexAttributeFormatExtensions
    {
        public static int ByteSize(this VertexAttributeFormat att)
        {
            switch (att)
            {
                case VertexAttributeFormat.Float32:
                return sizeof(float);

                case VertexAttributeFormat.Int32:
                return sizeof(int);
            }

            throw new NotImplementedException();
        }

        public static VertexAttribPointerType ToOpenGlAttribType(this VertexAttributeFormat att)
        {
            switch (att)
            {
                case VertexAttributeFormat.Float32:
                return VertexAttribPointerType.Float;

                case VertexAttributeFormat.Int32:
                return VertexAttribPointerType.Int;
            }

            throw new NotImplementedException();
        }
    }
}

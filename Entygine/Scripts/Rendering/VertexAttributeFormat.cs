using OpenToolkit.Graphics.OpenGL4;
using System;

namespace Entygine.Rendering
{
    public enum VertexAttributeFormat
    {
        Float32,
    }

    public static class VertexAttributeFormatExtensions
    {
        public static int ByteSize(this VertexAttributeFormat att)
        {
            switch (att)
            {
                case VertexAttributeFormat.Float32:
                return sizeof(float);
            }

            throw new NotImplementedException();
        }

        public static VertexAttribPointerType ToOpenGlAttribType(this VertexAttributeFormat att)
        {
            switch (att)
            {
                case VertexAttributeFormat.Float32:
                return VertexAttribPointerType.Float;
            }

            throw new NotImplementedException();
        }
    }
}

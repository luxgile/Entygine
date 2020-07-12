using SixLabors.ImageSharp;

namespace Entygine.Rendering
{
    public class VertexBufferLayout
    {
        private VertexAttribute att;
        private VertexAttributeFormat format;
        int size;

        public VertexBufferLayout(VertexAttribute att, VertexAttributeFormat format, int size)
        {
            this.att = att;
            this.format = format;
            this.size = size;
        }

        public VertexAttribute Attribute => att;
        public VertexAttributeFormat Format => format;
        public int Size => size;
    }
}

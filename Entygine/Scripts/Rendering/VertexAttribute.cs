namespace Entygine.Rendering
{
    public enum VertexAttribute
    {
        Position,
        Normal,
        Tangent,
        Color,
        Uv0,
        Uv1,
        Uv2,
        Uv3,
        Uv4,
        Uv5,
        Uv6,
        Uv7,
    }

    public static class VertexAttributeExtensions
    {
        public static int GetAttributeLocation(this VertexAttribute attribute, Material mat)
        {
            switch (attribute)
            {
                case VertexAttribute.Position:
                return mat.shader.GetAttributeLocation("aPosition");

                case VertexAttribute.Normal:
                return mat.shader.GetAttributeLocation("aNormal");

                case VertexAttribute.Uv0:
                return mat.shader.GetAttributeLocation("aTexCoord");
            }

            throw new System.NotImplementedException();
        }
    }
}

using OpenTK.Mathematics;

namespace Entygine.Rendering
{
    public abstract class Light
    {
        public abstract Framebuffer Framebuffer { get; }

        public abstract void BindShadowMap();
        public abstract void UnbindShadowMap();
        public abstract Matrix4 GetProjection();
    }
}

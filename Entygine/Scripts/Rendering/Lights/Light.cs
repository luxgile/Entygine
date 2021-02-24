using OpenTK.Mathematics;

namespace Entygine.Rendering
{
    public abstract class Light
    {
        public abstract DepthTexture Depthmap { get; }
        protected int DepthMapHandleFBO { get; }

        public Light()
        {
            DepthMapHandleFBO = Ogl.GenFramebuffer();
        }

        public abstract void BindShadowMap();
        public abstract Matrix4 GetProjection();
    }
}

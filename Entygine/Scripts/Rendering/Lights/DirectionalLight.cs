using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Entygine.Rendering
{
    public class DirectionalLight : Light
    {
        private CameraData camera;
        private Framebuffer multisampledFBO;
        private Framebuffer finalFBO;
        private const int TEXTURE_RES = 1024;

        public DirectionalLight() : base()
        {
            camera = CameraData.CreateOrthographicCamera(1, 20f, 0.1f, 100);
            multisampledFBO = new Framebuffer(new Mathematics.Vec2i(TEXTURE_RES, TEXTURE_RES), "Directional MSAA FBO");
            multisampledFBO.AddDepthBuffer(true);
            finalFBO = new Framebuffer(new Mathematics.Vec2i(TEXTURE_RES, TEXTURE_RES), "Directional Light FBO");
            finalFBO.AddDepthBuffer(false);
        }

        public override void BindShadowMap()
        {
            Ogl.Viewport(0, 0, TEXTURE_RES, TEXTURE_RES);
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, multisampledFBO.Handle);
        }

        public override void UnbindShadowMap()
        {
            Framebuffer.Blit(multisampledFBO, finalFBO, ClearBufferMask.DepthBufferBit);
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public override Matrix4 GetProjection()
        {
            return camera.CalculateProjection(true);
        }

        public override Framebuffer Framebuffer => finalFBO;
    }
}

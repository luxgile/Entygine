using OpenToolkit.Graphics.OpenGL4;
using OpenToolkit.Mathematics;

namespace Entygine.Rendering
{
    public class DirectionalLight : Light
    {
        private CameraData camera;
        private DepthTexture depthMap = new DepthTexture(TEXTURE_RES, TEXTURE_RES);

        private const int TEXTURE_RES = 1024;

        public DirectionalLight() : base()
        {
            camera = CameraData.CreateOrthographicCamera(1, 20f, 0.1f, 100);

            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, DepthMapHandleFBO);
            Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, depthMap.Handle, 0);
            Ogl.DrawBuffer(DrawBufferMode.None);
            Ogl.ReadBuffer(ReadBufferMode.None);
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public override void BindShadowMap()
        {
            Ogl.Viewport(0, 0, TEXTURE_RES, TEXTURE_RES);
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, DepthMapHandleFBO);
        }

        public override Matrix4 GetProjection()
        {
            return camera.CalculateProjection(true);
        }

        public override DepthTexture Depthmap => depthMap;
    }
}

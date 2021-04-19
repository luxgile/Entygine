using Entygine.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Entygine.Rendering.Pipeline
{
    public class ForwardRenderPipeline : IRenderPipeline
    {
        private int framebuffer_color;

        public ForwardRenderPipeline()
        {
            framebuffer_color = Ogl.GenFramebuffer("Forward Color");
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer_color);
            Ogl.DrawBuffer(DrawBufferMode.ColorAttachment0);
            Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Render(ref RenderContext context, CameraData[] cameras, Matrix4[] transforms)
        {
            context.CommandBuffer.QueueCommand(RenderCommandsLibrary.GenerateShadowMaps());

            for (int i = 0; i < cameras.Length; i++)
            {
                Matrix4 cameraTransform = transforms[i];
                CameraData camera = cameras[i];

                context.CommandBuffer.QueueCommand(new RenderCommand("Bind color FBO", (ref RenderContext context) =>
                {
                    Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer_color);
                    Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, TextureTarget.Texture2D, camera.DepthTargetTexture.Handle, 0);
                    Ogl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, TextureTarget.Texture2D, camera.ColorTargetTexture.handle, 0);
                    FramebufferErrorCode status = Ogl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
                    if (status != FramebufferErrorCode.FramebufferComplete)
                        DevTools.DevConsole.Log(status);

                    Ogl.Enable(EnableCap.DepthTest);
                    Ogl.Enable(EnableCap.CullFace);
                    Ogl.Enable(EnableCap.ProgramPointSize);
                    Ogl.PointSize(10f);
                }));

                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawGeometry(camera, cameraTransform));
                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawGizmos(camera, cameraTransform));
                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawSkybox(camera, cameraTransform));

                context.CommandBuffer.QueueCommand(new RenderCommand("Unbind FBO", (ref RenderContext context) =>
                {
                    Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                }));


            }

            //context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawUI());
        }
    }
}

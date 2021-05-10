using Entygine.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Entygine.Rendering.Pipeline
{
    public class ForwardRenderPipeline : IRenderPipeline
    {
        public void Render(ref RenderContext context, CameraData[] cameras, Matrix4[] transforms)
        {
            context.CommandBuffer.QueueCommand(RenderCommandsLibrary.GenerateShadowMaps());

            for (int i = 0; i < cameras.Length; i++)
            {
                Matrix4 cameraTransform = transforms[i];
                CameraData camera = cameras[i];

                context.CommandBuffer.QueueCommand(new RenderCommand("Bind color FBO", (ref RenderContext context) =>
                {
                    camera.Framebuffer.Bind();

                    Ogl.Enable(EnableCap.DepthTest);
                    Ogl.Enable(EnableCap.CullFace);
                    Ogl.Enable(EnableCap.ProgramPointSize);
                    Ogl.Enable(EnableCap.Multisample);
                    Ogl.PointSize(10f);
                }));

                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawGeometry(camera, cameraTransform));
                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawGizmos(camera, cameraTransform));
                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawSkybox(camera, cameraTransform));

                context.CommandBuffer.QueueCommand(new RenderCommand("Unbind FBO", (ref RenderContext context) =>
                {
                    Framebuffer.Blit(camera.Framebuffer, camera.FinalFramebuffer, ClearBufferMask.ColorBufferBit);
                    Ogl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                }));


            }

            //context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawUI());
        }
    }
}

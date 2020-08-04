using OpenToolkit.Mathematics;

namespace Entygine.Rendering.Pipeline
{
    public class DefaultRenderPipeline : IRenderPipeline
    {
        public void Render(ref RenderContext context, CameraData[] cameras, Matrix4[] transforms)
        {
            context.CommandBuffer.QueueCommand(RenderCommandsLibrary.ClearColorAndDepthBuffer());

            for (int i = 0; i < cameras.Length; i++)
                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawGeometry(cameras[i], transforms[i]));
        }
    }
}

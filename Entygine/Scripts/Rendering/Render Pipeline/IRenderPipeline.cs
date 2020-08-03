using OpenToolkit.Mathematics;

namespace Entygine.Rendering.Pipeline
{
    public interface IRenderPipeline
    {
        void Render(ref RenderContext context, CameraData[] camera, Matrix4[] transforms);
    }
}

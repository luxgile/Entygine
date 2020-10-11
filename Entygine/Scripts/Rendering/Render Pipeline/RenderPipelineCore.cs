using OpenTK.Mathematics;

namespace Entygine.Rendering.Pipeline
{
    public static class RenderPipelineCore
    {
        private static IRenderPipeline activePipeline;
        private static RenderContext renderContext;

        static RenderPipelineCore()
        {
            renderContext = new RenderContext(new RenderCommandBuffer());
            renderContext.AddData(new GeometryRenderData());
            renderContext.AddData(new SkyboxRenderData());
            renderContext.AddData(new UICanvasRenderData());
            renderContext.AddData(new LightsRenderData());
        }

        public static bool TryGetContext<T0>(out T0 context) where T0 : RenderContextData => renderContext.TryGetData<T0>(out context);

        public static void SetSkybox(Skybox skybox)
        {
            if (renderContext.TryGetData(out SkyboxRenderData skyboxRenderData))
                skyboxRenderData.skybox = skybox;
        }

        public static void Draw(CameraData[] cameras, Matrix4[] transforms)
        {
            renderContext.ClearBuffer();

            activePipeline.Render(ref renderContext, cameras, transforms);

            renderContext.CommandBuffer.Dispatch(ref renderContext);
        }

        public static void SetPipeline(IRenderPipeline pipeline)
        {
            activePipeline = pipeline;
        }
    }
}

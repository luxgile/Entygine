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
            renderContext.AddData(new GizmosContextData());
        }

        public static bool TryGetContext<T0>(out T0 context) where T0 : RenderContextData
        {
            ThreadUtils.ThrowWorkerThread();
            return renderContext.TryGetData(out context);
        }

        public static void SetSkybox(Skybox skybox)
        {
            ThreadUtils.ThrowWorkerThread();
            if (renderContext.TryGetData(out SkyboxRenderData skyboxRenderData))
                skyboxRenderData.skybox = skybox;
        }

        public static void Draw(CameraData[] cameras, Matrix4[] transforms)
        {
            ThreadUtils.ThrowWorkerThread();

            renderContext.ClearBuffer();

            activePipeline.Render(ref renderContext, cameras, transforms);

            renderContext.CommandBuffer.Dispatch(ref renderContext);
        }

        public static void SetPipeline(IRenderPipeline pipeline)
        {
            ThreadUtils.ThrowWorkerThread();

            activePipeline = pipeline;
        }
    }
}

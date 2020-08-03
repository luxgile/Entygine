﻿using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.OpenGL4;
using System.Collections.Generic;
using Entygine.DevTools;

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
        }

        public static void QueueMesh(Mesh mesh, Material mat, Matrix4 transform)
        {
            MeshRender renderMesh = new MeshRender() { mat = mat, mesh = mesh };
            if (renderContext.TryGetData(out GeometryRenderData geometryData))
                geometryData.AddMesh(renderMesh, transform);
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

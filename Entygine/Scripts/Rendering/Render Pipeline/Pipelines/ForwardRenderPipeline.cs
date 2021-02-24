﻿using OpenTK.Mathematics;

namespace Entygine.Rendering.Pipeline
{
    public class ForwardRenderPipeline : IRenderPipeline
    {
        public void Render(ref RenderContext context, CameraData[] cameras, Matrix4[] transforms)
        {
            context.CommandBuffer.QueueCommand(RenderCommandsLibrary.GenerateShadowMaps());

            for (int i = 0; i < cameras.Length; i++)
            {
                CameraData camera = cameras[i];
                Matrix4 cameraTransform = transforms[i];

                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawGeometry(camera, cameraTransform));
                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawGizmos(camera, cameraTransform));
                context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawSkybox(camera, cameraTransform));
            }

            context.CommandBuffer.QueueCommand(RenderCommandsLibrary.DrawUI());
        }
    }
}

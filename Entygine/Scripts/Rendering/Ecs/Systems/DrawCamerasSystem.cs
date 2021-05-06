using Entygine.Cycles;
using Entygine.Ecs.Components;
using Entygine.Mathematics;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Ecs.Systems
{
    [SystemGroup(typeof(MainPhases.DefaultPhaseId), PhaseType.Render)]
    public class DrawCamerasSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(TypeCache.ReadType(typeof(C_Camera)), TypeCache.ReadType(typeof(C_Transform)));

        protected override QueryScope SetupQuery()
        {
            return new ChunkQueryScope(settings, (context) =>
            {
                int entityCount = context.GetEntityCount();
                CameraData[] cameraDatas = new CameraData[entityCount];
                Matrix4[] cameraTransforms = new Matrix4[entityCount];
                for (int c = 0; c < entityCount; c++)
                {
                    context.ReadComponent(c, out C_Camera cam);
                    if (cam.cameraData.Framebuffer == null)
                    {
                        Vec2i res = AppScreen.Resolution;
                        var fb = new Framebuffer(res, "Camera FBO");
                        fb.AddColorBuffer(true);
                        fb.AddDepthBuffer(true);
                        cam.cameraData.SetFramebuffer(fb);


                        var ffb = new Framebuffer(res, "Camera Final FBO");
                        ffb.AddColorBuffer(false);
                        cam.cameraData.SetFinalFramebuffer(ffb);

                        context.WriteComponent(c, cam);
                    }
                    else if (cam.cameraData.Framebuffer.Size != AppScreen.Resolution)
                    {
                        cam.cameraData.Framebuffer.ChangeSize(AppScreen.Resolution);
                        cam.cameraData.FinalFramebuffer.ChangeSize(AppScreen.Resolution);
                    }

                    cameraDatas[c] = cam.cameraData;

                    context.ReadComponent(c, out C_Transform transform);
                    cameraTransforms[c] = transform.value;
                }

                RenderPipelineCore.Draw(cameraDatas, cameraTransforms);
            });
        }
    }
}

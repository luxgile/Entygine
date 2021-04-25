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
    public class S_DrawCameras : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.With(TypeCache.ReadType(typeof(C_Camera)), TypeCache.ReadType(typeof(C_Transform)));
            IterateQuery(new Iterator(), query, false);
        }

        private struct Iterator : IQueryChunkIterator
        {
            public void Iteration(ref EntityChunk chunk)
            {
                if (!chunk.TryGetComponents<C_Camera>(out ComponentArray cameras) || !chunk.TryGetComponents<C_Transform>(out ComponentArray transforms))
                    return;

                CameraData[] cameraDatas = new CameraData[chunk.Count];
                Matrix4[] cameraTransforms = new Matrix4[chunk.Count];
                for (int c = 0; c < chunk.Count; c++)
                {
                    C_Camera cam = cameras.Get<C_Camera>(c);
                    if(cam.cameraData.Framebuffer == null)
                    {
                        Vec2i res = AppScreen.Resolution;
                        var fb = new Framebuffer(res, "Camera FBO");
                        fb.AddColorBuffer();
                        fb.AddDepthBuffer();
                        cam.cameraData.SetFramebuffer(fb);


                        var ffb = new Framebuffer(res, "Camera Final FBO");
                        ffb.AddColorBuffer();
                        cam.cameraData.SetFinalFramebuffer(ffb);
                        chunk.SetComponent(c, cam);
                    }
                    else if(cam.cameraData.Framebuffer.Size != AppScreen.Resolution)
                    {
                        cam.cameraData.Framebuffer.ChangeSize(AppScreen.Resolution);
                        cam.cameraData.FinalFramebuffer.ChangeSize(AppScreen.Resolution);
                    }
                    //if(cam.cameraData.ColorTargetTexture == null)
                    //{
                    //    Vec2i res = AppScreen.Resolution;
                    //    cam.cameraData.SetColorTargetTexture(new Texture2D(res.x, res.y, "Camera Color FBO"));
                    //    cam.cameraData.SetDepthTargetTexture(new DepthTexture(res.x, res.y, "Camera Depth"));
                    //    chunk.SetComponent(c, cam);
                    //}
                    //else if (cam.cameraData.ColorTargetTexture.Size != AppScreen.Resolution)
                    //{
                    //    cam.cameraData.ColorTargetTexture.SetSize(AppScreen.Resolution);
                    //    cam.cameraData.DepthTargetTexture.SetSize(AppScreen.Resolution);
                    //}

                    cameraDatas[c] = cam.cameraData;
                    cameraTransforms[c] = transforms.Get<C_Transform>(c).value;
                }

                RenderPipelineCore.Draw(cameraDatas, cameraTransforms);
            }
        }
    }
}

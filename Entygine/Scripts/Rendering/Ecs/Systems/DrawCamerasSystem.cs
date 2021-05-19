using Entygine.Cycles;
using Entygine.Ecs.Components;
using Entygine.Mathematics;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;
using OpenTK.Mathematics;

namespace Entygine.Ecs.Systems
{
    [SystemGroup(typeof(MainPhases.DefaultPhaseId), PhaseType.Render)]
    public class DrawCamerasSystem : QuerySystem<ChunkIterator>
    {
        protected override bool CheckChanges => false;
        protected override void OnFrame(float dt)
        {
            Iterator.With(C_Camera.Identifier, C_Transform.Identifier).Iterate((chunk) =>
            {
                int entityCount = chunk.Count;
                CameraData[] cameraDatas = new CameraData[entityCount];
                Matrix4[] cameraTransforms = new Matrix4[entityCount];
                chunk.TryGetComponents(C_Camera.Identifier, out ComponentArray cameras);
                chunk.TryGetComponents(C_Transform.Identifier, out ComponentArray transforms);
                for (int c = 0; c < entityCount; c++)
                {
                    ref C_Camera cam = ref cameras.GetRef<C_Camera>(c);
                    ref C_Transform transform = ref transforms.GetRef<C_Transform>(c);
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
                    }
                    else if (cam.cameraData.Framebuffer.Size != AppScreen.Resolution)
                    {
                        cam.cameraData.Framebuffer.ChangeSize(AppScreen.Resolution);
                        cam.cameraData.FinalFramebuffer.ChangeSize(AppScreen.Resolution);
                    }

                    cameraDatas[c] = cam.cameraData;
                    cameraTransforms[c] = transform.value;
                }

                RenderPipelineCore.Draw(cameraDatas, cameraTransforms);
            });
        }
    }
}

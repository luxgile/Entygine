using Entygine.Cycles;
using Entygine.Ecs.Components;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;
using OpenToolkit.Mathematics;
using System.Collections.Generic;

namespace Entygine.Ecs.Systems
{
    [SystemGroup(typeof(DefaultPhaseId), PhaseType.Render)]
    public class S_DrawCameras : BaseSystem
    {
        private EntityArchetype cameraArchetype = new EntityArchetype(typeof(C_Camera), typeof(C_Transform));
        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            bool forceMeshUpdate = MainDevWindowGL.Window.KeyboardState.IsKeyDown(OpenToolkit.Windowing.Common.Input.Key.ControlLeft);

            List<int> chunks = World.EntityManager.GetChunksWith(cameraArchetype, true);
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref World.EntityManager.GetChunk(chunks[i]);
                if (chunk.TryGetComponents<C_Transform>(out ComponentArray transforms) && chunk.TryGetComponents<C_Camera>(out ComponentArray cameras))
                {
                    CameraData[] cameraDatas = new CameraData[chunk.Count];
                    Matrix4[] cameraTransforms = new Matrix4[chunk.Count];
                    for (int c = 0; c < chunk.Count; c++)
                    {
                        cameraDatas[c] = cameras.Get<C_Camera>(c).cameraData;
                        cameraTransforms[c] = transforms.Get<C_Transform>(c).value;
                    }

                    RenderPipelineCore.Draw(cameraDatas, cameraTransforms);
                }
            }
        }
    }
}

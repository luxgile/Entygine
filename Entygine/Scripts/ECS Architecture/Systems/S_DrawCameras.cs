using Entygine.Cycles;
using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;
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

            List<EntityChunk> chunks = World.EntityManager.GetChunksWith(cameraArchetype, true);
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (chunk.TryGetComponents<C_Transform>(out ComponentArray transforms) && chunk.TryGetComponents<C_Camera>(out ComponentArray cameras))
                {
                    for (int c = 0; c < chunk.Count; c++)
                        RenderPipeline.Draw(((C_Camera)cameras[i]).cameraData, ((C_Transform)transforms[i]).value);
                }
            }
        }
    }
}

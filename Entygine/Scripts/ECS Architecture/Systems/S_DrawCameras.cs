using Entygine.Cycles;
using Entygine.Ecs.Components;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;
using OpenToolkit.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Ecs.Systems
{
    [SystemGroup(typeof(MainPhases.DefaultPhaseId), PhaseType.Render)]
    public class S_DrawCameras : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

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
                    cameraDatas[c] = cameras.Get<C_Camera>(c).cameraData;
                    cameraTransforms[c] = transforms.Get<C_Transform>(c).value;
                }

                RenderPipelineCore.Draw(cameraDatas, cameraTransforms);
            }
        }
    }
}

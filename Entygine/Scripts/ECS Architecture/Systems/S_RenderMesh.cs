using Entygine.Ecs.Components;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;
using OpenTK;
using OpenTK.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace Entygine.Ecs.Systems
{
    public class S_RenderMesh : RenderSystem
    {
        private EntityArchetype meshArchetype = new EntityArchetype(typeof(SC_RenderMesh), typeof(C_Transform));

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            List<EntityChunk> chunks = World.EntityManager.GetChunksWith(meshArchetype, true);
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (chunk.TryGetSharedComponents(out SC_RenderMesh renderMesh))
                {
                    if (chunk.HasChanged(LastVersionWorked))
                    {
                        chunk.ChunkVersion = World.EntityManager.Version;
                        if (chunk.TryGetComponents<C_Transform>(out ComponentArray transforms))
                        {
                            for (int c = 0; c < chunk.Count; c++)
                                RenderPipeline.QueueMesh(renderMesh.mesh, renderMesh.material, ((C_Transform)transforms[c]).value);
                        }
                    }
                }
            }
        }
    }
}

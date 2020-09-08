using Entygine.Ecs.Components;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;
using OpenToolkit.Mathematics;
using OpenToolkit.Graphics.ES30;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace Entygine.Ecs.Systems
{
    public class S_RenderMesh : BaseSystem
    {
        private EntityArchetype meshArchetype = new EntityArchetype(typeof(SC_RenderMesh), typeof(C_Transform));
        private EntityQuery query;

        protected override void OnSystemCreated()
        {
            base.OnSystemCreated();

            query = new EntityQuery(World).With(TypeCache.ReadType(typeof(SC_RenderMesh)), TypeCache.WriteType(typeof(C_Transform)));
        }

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.Perform(new Iterator(), LastVersionWorked);
        }

        private struct Iterator : IQueryEntityIterator
        {
            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (chunk.TryGetSharedComponent(out SC_RenderMesh renderMesh) && chunk.TryGetComponent(index, out C_Transform transform))
                    RenderPipelineCore.QueueMesh(renderMesh.mesh, renderMesh.material, transform.value);
            }
        }
    }
}

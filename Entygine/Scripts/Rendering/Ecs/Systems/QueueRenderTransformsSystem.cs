﻿using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    public class QueueRenderTransformsSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(TypeCache.ReadType(typeof(SC_RenderMesh)), TypeCache.WriteType(typeof(C_Transform)));

        protected override QueryScope SetupQuery()
        {
            return new ChunkQueryScope(settings, (context) =>
            {
                context.Read(out SC_RenderMesh renderMesh);

                if (!RenderPipelineCore.TryGetContext(out Rendering.GeometryRenderData geometryData))
                    return;

                if (geometryData.TryGetRenderMeshGroup(renderMesh.id, out Rendering.MeshRenderGroup group))
                {
                    group.ClearTransforms();
                    int entityCount = context.GetEntityCount();
                    for (int i = 0; i < entityCount; i++)
                    {
                        context.ReadComponent(i, out C_Transform transform);
                        group.AddTransform(transform.value);
                    }
                }
            });
        }
    }
}

using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    public class S_QueueRenderTransforms : BaseSystem
    {
        private EntityQuerySettings query = new EntityQuerySettings();

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.With(TypeCache.ReadType(typeof(SC_RenderMesh)), TypeCache.WriteType(typeof(C_Transform)));
            IterateQuery(new Iterator(), query);
        }

        private struct Iterator : IQueryChunkIterator
        {
            public void Iteration(ref EntityChunk chunk)
            {
                if (!chunk.TryGetSharedComponent(out SC_RenderMesh renderMesh))
                    return;

                if (!RenderPipelineCore.TryGetContext(out Rendering.GeometryRenderData geometryData))
                    return;

                if (geometryData.TryGetRenderMeshGroup(renderMesh.id, out Rendering.MeshRenderGroup group))
                {
                    group.ClearTransforms();
                    for (int i = 0; i < chunk.Count; i++)
                    {
                        if (chunk.TryGetComponent(i, out C_Transform transform))
                            group.AddTransform(transform.value);
                    }
                }
            }
        }
    }
}

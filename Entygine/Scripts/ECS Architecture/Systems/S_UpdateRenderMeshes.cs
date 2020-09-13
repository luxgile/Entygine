using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    [BeforeSystem(typeof(S_QueueRenderTransforms))]
    public class S_UpdateRenderMeshes : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            query.With(TypeCache.WriteType(typeof(SC_RenderMesh)));
            IterateQuery(new Iterator2(), query);
        }

        private struct Iterator2 : IQueryChunkIterator
        {
            public void Iteration(ref EntityChunk chunk)
            {
                if (!chunk.TryGetSharedComponent(out SC_RenderMesh renderMesh))
                    return;

                if (!RenderPipelineCore.TryGetContext(out Rendering.GeometryRenderData geometryData))
                    return;

                if (!geometryData.TryGetRenderMeshGroup(renderMesh.id, out Rendering.MeshRenderGroup group))
                    renderMesh.id = geometryData.CreateRenderMeshGroup(renderMesh.value, out group);
                else
                    group.MeshRender = renderMesh.value;

                chunk.SetSharedComponent(renderMesh);
            }
        }
    }
}

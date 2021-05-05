using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    [BeforeSystem(typeof(S_QueueRenderTransforms))]
    public class UpdateRenderMeshesSystem : QuerySystem
    {
        private QuerySettings settings = new QuerySettings().With(TypeCache.WriteType(typeof(SC_RenderMesh)));

        protected override QueryScope SetupQuery()
        {
            return new ChunkQueryScope(settings, (context) =>
            {
                context.Read(out SC_RenderMesh renderMesh);

                if (!RenderPipelineCore.TryGetContext(out Rendering.GeometryRenderData geometryData))
                    return;

                if (!geometryData.TryGetRenderMeshGroup(renderMesh.id, out Rendering.MeshRenderGroup group))
                    renderMesh.id = geometryData.CreateRenderMeshGroup(renderMesh.value, out group);
                else
                    group.MeshRender = renderMesh.value;

                context.Write(renderMesh);
            });
        }

        private struct Iterator : IQueryChunkIterator
        {
            public void Iteration(ref EntityChunk chunk)
            {
            }
        }
    }
}

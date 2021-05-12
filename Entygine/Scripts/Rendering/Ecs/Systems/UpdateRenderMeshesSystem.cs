using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    [BeforeSystem(typeof(QueueRenderTransformsSystem))]
    public class UpdateRenderMeshesSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(SC_RenderMesh.Identifier);

        protected override QueryScope SetupQuery()
        {
            return new ChunkQueryScope(settings, (ref ChunkQueryContext context) =>
            {
                context.Read(out SC_RenderMesh renderMesh);

                if (!RenderPipelineCore.TryGetContext(out Rendering.GeometryRenderData geometryData))
                    return;

                if (!geometryData.TryGetRenderMeshGroup(renderMesh.id, out Rendering.MeshRenderGroup group))
                    renderMesh.id = geometryData.CreateRenderMeshGroup(renderMesh.value, out group);
                else
                    group.MeshRender = renderMesh.value;

                context.Write(SC_RenderMesh.Identifier, renderMesh);
            });
        }
    }
}

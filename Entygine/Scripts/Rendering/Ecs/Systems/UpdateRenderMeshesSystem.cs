using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    [BeforeSystem(typeof(QueueRenderTransformsSystem))]
    public class UpdateRenderMeshesSystem : QuerySystem
    {
        protected override bool RunAsync => false;

        protected override void OnFrame(float dt)
        {
            if (!RenderPipelineCore.TryGetContext(out Rendering.GeometryRenderData geometryData))
                return;
            
            Iterator.RWith(SC_RenderMesh.Identifier).Iterate((chunk) =>
            {
                chunk.TryGetSharedComponent(SC_RenderMesh.Identifier, out SC_RenderMesh renderMesh);

                if (!geometryData.TryGetRenderMeshGroup(renderMesh.id, out Rendering.MeshRenderGroup group))
                    renderMesh.id = geometryData.CreateRenderMeshGroup(renderMesh.value, out group);
                else
                    group.MeshRender = renderMesh.value;

                chunk.SetSharedComponent(SC_RenderMesh.Identifier, renderMesh);
            });
        }
    }
}

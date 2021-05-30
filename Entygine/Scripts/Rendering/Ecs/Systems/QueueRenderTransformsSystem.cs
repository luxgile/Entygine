using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    public class QueueRenderTransformsSystem : QuerySystem
    {
        protected override bool RunAsync => false;
        protected override void OnFrame(float dt)
        {
            if (!RenderPipelineCore.TryGetContext(out Rendering.GeometryRenderData geometryData))
                return;

            Iterator.RWith(SC_RenderMesh.Identifier, C_Transform.Identifier).Iterate((chunk) =>
            {
                chunk.TryGetSharedComponent(SC_RenderMesh.Identifier, out SC_RenderMesh renderMesh);

                if (geometryData.TryGetRenderMeshGroup(renderMesh.id, out Rendering.MeshRenderGroup group))
                {
                    group.ClearTransforms();
                    chunk.TryGetComponents(C_Transform.Identifier, out ComponentArray transforms);
                    for (int i = 0; i < chunk.Count; i++)
                    {
                        group.AddTransform(transforms.Get<C_Transform>(i).value);
                    }
                }
            });
        }
    }
}

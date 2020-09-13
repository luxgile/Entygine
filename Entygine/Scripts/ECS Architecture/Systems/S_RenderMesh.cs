using Entygine.Ecs.Components;
using Entygine.Rendering.Pipeline;

namespace Entygine.Ecs.Systems
{
    public class S_RenderMesh : BaseSystem
    {
        private EntityQuery query = new EntityQuery().With(TypeCache.ReadType(typeof(SC_RenderMesh)), TypeCache.WriteType(typeof(C_Transform)));

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            IterateQuery(new Iterator(), query);
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

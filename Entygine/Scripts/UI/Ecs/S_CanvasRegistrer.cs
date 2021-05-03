using Entygine.Cycles;
using Entygine.Ecs;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class S_CanvasRegistrer : BaseSystem
    {
        private EntityQuerySettings query = new EntityQuerySettings();

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.With(TypeCache.ReadType(typeof(C_UICanvas)));

            IterateQuery(new Iterator(), query, true);
        }

        private struct Iterator : IQueryChunkIterator
        {
            public void Iteration(ref EntityChunk chunk)
            {
                if (!RenderPipelineCore.TryGetContext(out UICanvasRenderData canvasData))
                    return;

                canvasData.ClearCanvas();

                for (int i = 0; i < chunk.Count; i++)
                {
                    if (chunk.TryGetComponent(i, out C_UICanvas canvas))
                        canvasData.AddCanvas(canvas.canvas);
                }
            }
        }
    }
}

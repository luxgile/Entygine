using Entygine.Cycles;
using Entygine.Ecs;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class CanvasRegistrerSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(C_UICanvas.Identifier);

        protected override QueryScope SetupQuery()
        {
            return new ChunkQueryScope(settings, (ref ChunkQueryContext context) =>
            {
                if (!RenderPipelineCore.TryGetContext(out UICanvasRenderData canvasData))
                    return;

                canvasData.ClearCanvas();

                int entityCount = context.GetEntityCount();
                for (int i = 0; i < entityCount; i++)
                {
                    context.ReadComponent(i, C_UICanvas.Identifier, out C_UICanvas canvas);
                    canvasData.AddCanvas(canvas.canvas);
                }
            });
        }
    }
}

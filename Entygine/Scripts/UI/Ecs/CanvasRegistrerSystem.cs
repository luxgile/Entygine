using Entygine.Cycles;
using Entygine.Ecs;
using Entygine.Rendering;
using Entygine.Rendering.Pipeline;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class CanvasRegistrerSystem : QuerySystem
    {
        protected override void OnFrame(float dt)
        {
            if (!RenderPipelineCore.TryGetContext(out UICanvasRenderData canvasData))
                return;

            canvasData.ClearCanvas();

            Iterator.With(C_UICanvas.Identifier).Iterate((chunk) =>
            {
                chunk.TryGetComponents(C_UICanvas.Identifier, out ComponentArray array);
                for (int i = 0; i < chunk.Count; i++)
                {
                    ref C_UICanvas canvas = ref array.GetRef<C_UICanvas>(i);
                    canvasData.AddCanvas(canvas.canvas);
                }
            });
        }
    }
}

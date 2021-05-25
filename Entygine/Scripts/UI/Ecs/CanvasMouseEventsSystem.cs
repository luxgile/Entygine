using Entygine.Cycles;
using Entygine.DevTools;
using Entygine.Ecs;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    internal class CanvasMouseEventsSystem : QuerySystem<EntygineGeneratedIterators>
    {
        protected override void OnFrame(float dt)
        {
            var mouseData = new MouseData();
            Iterator.Iterate((ref C_UICanvas canvas) =>
            {
                canvas.canvas.TriggerMouseEvent(mouseData);
            });
        }
    }
}

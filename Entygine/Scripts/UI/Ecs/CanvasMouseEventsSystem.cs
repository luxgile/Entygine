using Entygine.Cycles;
using Entygine.DevTools;
using Entygine.Ecs;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class CanvasMouseEventsSystem : QuerySystem<EntityIterator>
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

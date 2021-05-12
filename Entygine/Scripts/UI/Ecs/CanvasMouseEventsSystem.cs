using Entygine.Cycles;
using Entygine.DevTools;
using Entygine.Ecs;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class CanvasMouseEventsSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(C_UICanvas.Identifier);
        private MouseData mouseData;

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            mouseData = new MouseData();
        }

        protected override QueryScope SetupQuery()
        {
            return new EntityQueryScope(settings, (ref EntityQueryContext context) =>
            {
                context.Read(C_UICanvas.Identifier, out C_UICanvas canvas);
                canvas.canvas.TriggerMouseEvent(mouseData);
            });
        }
    }
}

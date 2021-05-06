using Entygine.Cycles;
using Entygine.DevTools;
using Entygine.Ecs;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class CanvasMouseEventsSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(TypeCache.ReadType<C_UICanvas>());
        private MouseData mouseData;

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            mouseData = new MouseData();
        }

        protected override QueryScope SetupQuery()
        {
            return new EntityQueryScope(settings, (context) =>
            {
                context.Read(out C_UICanvas canvas);
                canvas.canvas.TriggerMouseEvent(mouseData);
            });
        }
    }
}

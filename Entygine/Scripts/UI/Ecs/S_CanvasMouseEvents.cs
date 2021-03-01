using Entygine.Cycles;
using Entygine.Ecs;

namespace Entygine.UI
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId), PhaseType.Logic)]
    public class S_CanvasMouseEvents : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.With(TypeCache.ReadType<C_UICanvas>());

            MouseData mouseData = new MouseData();
            mouseData.positionDelta = MainDevWindowGL.Window.MouseState.Delta;

            
            Iterator it = new Iterator();
            IterateQuery(it, query, false);
        }

        private struct Iterator : IQueryEntityIterator
        {
            public MouseData mouseData;

            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (chunk.TryGetComponent(index, out C_UICanvas canvas))
                    canvas.canvas.TriggerMouseEvent(mouseData);
            }
        }
    }
}

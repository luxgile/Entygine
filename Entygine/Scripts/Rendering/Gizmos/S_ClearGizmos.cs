using Entygine.Cycles;
using Entygine.Ecs;
using Entygine.Rendering.Pipeline;

namespace Entygine.DevTools
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId))]
    public class S_ClearGizmos : BaseSystem
    {
        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            if (RenderPipelineCore.TryGetContext(out GizmosContextData gizmosData))
                gizmosData.Clear();
        }
    }

    [SystemGroup(typeof(MainPhases.LatePhaseId))]
    public class S_DispatchGizmos : BaseSystem
    {
        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            DevGizmos.Dispatch();
        }
    }
}

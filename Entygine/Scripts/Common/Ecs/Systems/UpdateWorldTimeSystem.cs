using Entygine.Cycles;
using Entygine.Ecs;

namespace Entygine
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId))]
    public class UpdateWorldTimeSystem : BaseSystem
    {
        protected override void OnPerformFrame(float dt)
        {
            ref C_WorldTime time = ref GetSingleton<C_WorldTime>(C_WorldTime.Identifier);
            time.value += dt;
        }
    }
}

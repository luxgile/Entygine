using Entygine.Cycles;
using Entygine.Ecs;

namespace Entygine
{
    [SystemGroup(typeof(MainPhases.EarlyPhaseId))]
    public class UpdateWorldTimeSystem : BaseSystem
    {
        protected override void OnPerformFrame(float dt)
        {
            ref WorldTimeComponent time = ref GetSingleton<WorldTimeComponent>(WorldTimeComponent.Identifier);
            time.value += dt;
        }
    }
}

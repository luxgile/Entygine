using Entygine.Cycles;

namespace Entygine.Ecs
{
    public struct EcsPhaseId : IPhaseId
    {
        public static EcsPhaseId Default => new EcsPhaseId();
    }
}

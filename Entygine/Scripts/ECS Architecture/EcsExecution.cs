using Entygine.Cycles;

namespace Entygine.Ecs
{
    public class EcsExecution
    {
        public EcsExecution(WorkerCycleCore workerCore)
        {
            if(workerCore.TryFindFirst<DefaultPhaseId>(out WorkerPhase normalPhase))
            {
                var subPhases = normalPhase.GetPhases();
                WorkerPhase defaultPhase = new WorkerPhase(EcsPhaseId.Default, "Default Ecs Execution");
                subPhases.Add(defaultPhase);
                normalPhase.SetPhases(subPhases);
            }

            if (workerCore.TryFindFirst<DefaultPhaseId>(out WorkerPhase latePhase))
            {
                var subPhases = latePhase.GetPhases();
                WorkerPhase latePhaseEcs = new WorkerPhase(EcsPhaseId.Default, "Late Ecs Execution");
                subPhases.Add(latePhaseEcs);
                latePhase.SetPhases(subPhases);
            }
        }
    }
}

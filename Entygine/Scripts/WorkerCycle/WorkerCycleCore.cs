using NUnit.Framework;
using System.Collections.Generic;
using System.Data;

namespace Entygine.Cycles
{
    public class WorkerCycleCore
    {
        private struct RootPhaseId : IPhaseId { public static RootPhaseId Default => new RootPhaseId(); }

        private WorkerPhase root;

        public WorkerCycleCore()
        {
            root = new WorkerPhase(RootPhaseId.Default, "Root Phase");
            List<WorkerPhase> phases = new List<WorkerPhase>()
            {
                new WorkerPhase(EarlyPhaseId.Default, "Early Phase"),
                new WorkerPhase(DefaultPhaseId.Default, "Default Phase"),
                new WorkerPhase(LatePhaseId.Default, "Late Phase"),
            };
            root.SetPhases(phases);
        }

        public bool TryFindFirst<T0>(out WorkerPhase phase) where T0 : IPhaseId
        {
            if(EvaluatePhase(ref root))
            {
                phase = root;
                return true;
            }

            phase = default;
            return false;

            bool EvaluatePhase(ref WorkerPhase phase)
            {
                if (phase.IsPhase<T0>())
                    return true;

                List<WorkerPhase> subPhases = phase.GetPhases();
                for (int i = 0; i < subPhases.Count; i++)
                {
                    WorkerPhase currPhase = subPhases[i];
                    if (EvaluatePhase(ref currPhase))
                        return true;
                }

                return false;
            }
        }

        public void PerformCycle()
        {
            root.PerformPhase();
        }
    }
}

using Entygine.DevTools;
using System;
using System.Collections.Generic;

namespace Entygine.Cycles
{
    public delegate void WorkerPhaseModifierDelegate(ref WorkerPhase phase);
    public struct RootPhaseId : IPhaseId { public static RootPhaseId Default => new RootPhaseId(); }
    public class WorkerCycleCore
    {

        private WorkerPhase logicRoot;
        private WorkerPhase renderRoot;

        internal WorkerCycleCore()
        {
            //Logic Phase
            logicRoot = new WorkerPhase(RootPhaseId.Default, "Logic Root Phase");

            List<WorkerPhase> phases = new List<WorkerPhase>()
            {
                new WorkerPhase(MainPhases.EarlyPhaseId.Default, "Early Phase"),
                new WorkerPhase(MainPhases.DefaultPhaseId.Default, "Default Phase"),
                new WorkerPhase(MainPhases.LatePhaseId.Default, "Late Phase"),
            };
            logicRoot.SetPhases(phases.ToArray());

            //Render Phase
            renderRoot = new WorkerPhase(RootPhaseId.Default, "Render Root Phase");
        }

        public void FindFirstLogicPhaseAndModify<T0>(WorkerPhaseModifierDelegate callback) where T0 : IPhaseId
            => FindFirstPhaseAndModify(ref logicRoot, typeof(T0), callback);
        public void FindFirstRenderPhaseAndModify<T0>(WorkerPhaseModifierDelegate callback) where T0 : IPhaseId
            => FindFirstPhaseAndModify(ref renderRoot, typeof(T0), callback);

        private void FindFirstPhaseAndModify(ref WorkerPhase workerPhase, Type phaseId, WorkerPhaseModifierDelegate callback)
        {
            if (workerPhase.IsPhase(phaseId))
                callback(ref workerPhase);
            else
            {
                workerPhase.FindFirstPhaseAndModify(phaseId, callback);
            }
        }

        internal void PerformLogicCycle(float deltaTime)
        {
            logicRoot.PerformPhase(deltaTime);
        }

        internal void PerformRenderCycle(float deltaTime)
        {
            renderRoot.PerformPhase(deltaTime);
        }
    }
}

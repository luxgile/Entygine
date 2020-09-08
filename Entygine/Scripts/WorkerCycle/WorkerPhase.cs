using System;
using System.Collections.Generic;

namespace Entygine.Cycles
{
    public struct WorkerPhase
    {
        private string name;
        private IPhaseId id;
        private WorkerPhase[] subPhases;

        private Action<float> phasePerformed;

        public WorkerPhase(IPhaseId id, string name)
        {
            this.name = name;
            this.id = id;
            subPhases = new WorkerPhase[0];
            phasePerformed = null;
        }

        public void SetCallback(Action<float> callback)
        {
            phasePerformed = callback;
        }

        public void PerformPhase(float deltaTime)
        {
            try { phasePerformed?.Invoke(deltaTime); }
            catch (Exception e) { Console.WriteLine(e); }

            try 
            {
                for (int i = 0; i < subPhases.Length; i++)
                    subPhases[i].PerformPhase(deltaTime);
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public bool IsPhase<T>() where T : IPhaseId => IsPhase(typeof(T));

        internal void FindFirstPhaseAndModify(Type phaseId, WorkerPhaseModifierDelegate callback)
        {
            for (int i = 0; i < subPhases.Length; i++)
            {
                if (subPhases[i].IsPhase(phaseId))
                    callback(ref subPhases[i]);
                else
                    subPhases[i].FindFirstPhaseAndModify(phaseId, callback);
            }
        }

        public bool IsPhase(Type type)
        {
            return id.GetType() == type;
        }

        public WorkerPhase[] GetPhases() => subPhases;
        public void SetPhases(WorkerPhase[] phases) => subPhases = phases;
    }
}

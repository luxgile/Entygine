using System;
using System.Collections.Generic;

namespace Entygine.Cycles
{
    public struct WorkerPhase
    {
        private string name;
        private IPhaseId id;
        private List<WorkerPhase> subPhases;

        private Action phasePerformed;

        public WorkerPhase(IPhaseId id, string name)
        {
            this.name = name;
            this.id = id;
            subPhases = new List<WorkerPhase>();
            phasePerformed = null;
        }

        public void SetCallback(Action callback)
        {
            phasePerformed = callback;
        }

        public void PerformPhase()
        {
            try { phasePerformed?.Invoke(); }
            catch (Exception e) { Console.WriteLine(e); }

            try 
            {
                for (int i = 0; i < subPhases.Count; i++)
                    subPhases[i].PerformPhase();
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public bool IsPhase<T>() where T : IPhaseId
        {
            return id is T;
        }

        public List<WorkerPhase> GetPhases() => subPhases;
        public void SetPhases(List<WorkerPhase> phases) => subPhases = phases;
    }
}

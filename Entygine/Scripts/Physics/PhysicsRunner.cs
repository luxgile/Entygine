using Entygine.Cycles;
using System;
using System.Collections.Generic;

namespace Entygine.Physics
{
    public class PhysicsRunner
    {
        public struct PhysicsPhaseId : IPhaseId { public static PhysicsPhaseId Default = new PhysicsPhaseId(); }

        private List<PhysicsWorld> worlds = new List<PhysicsWorld>();

        private float stepTime = 0.02f;
        public float StepTime
        {
            get => stepTime;
            set
            {
                stepTime = value;
                if (stepTime <= Mathematics.Math.Epsilon)
                    stepTime = 0.001f;
            }
        }

        private float currentTime;

        public void AddWorld(PhysicsWorld world)
        {
            worlds.Add(world);
        }

        public PhysicsRunner AssignToWorker(WorkerCycleCore workerCore)
        {
            SetupPhysicsPhase<MainPhases.DefaultPhaseId>(workerCore, OnPhysicsPhase, "Physics Execution");

            return this;

            static void SetupPhysicsPhase<T0>(WorkerCycleCore workerCore, Action<float> callback, string name) where T0 : IPhaseId
            {
                workerCore.FindFirstLogicPhaseAndModify<T0>((ref WorkerPhase phase) =>
                {
                    WorkerPhase[] subPhases = phase.GetPhases();
                    WorkerPhase ecsPhase = new WorkerPhase(PhysicsPhaseId.Default, name);
                    ecsPhase.SetCallback(callback);
                    Array.Resize(ref subPhases, subPhases.Length + 1);
                    subPhases[^1] = ecsPhase;
                    phase.SetPhases(subPhases);
                });
            }
        }

        private void OnPhysicsPhase(float dt)
        {
            currentTime += dt;

            while(currentTime > StepTime)
            {
                currentTime -= StepTime;
                for (int i = 0; i < worlds.Count; i++)
                    worlds[i].StepPhysics(StepTime);
            }
        }

    }
}

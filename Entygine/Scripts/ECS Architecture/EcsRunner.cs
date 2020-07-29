using Entygine.Cycles;
using Entygine.DevTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entygine.Ecs
{
    public class EcsRunner
    {
        private List<BaseSystem> earlySystems = new List<BaseSystem>();
        private List<BaseSystem> defaultSystems = new List<BaseSystem>();
        private List<BaseSystem> lateSystems = new List<BaseSystem>();
        private List<BaseSystem> renderSystems = new List<BaseSystem>();

        public EcsRunner AssignToWorker(WorkerCycleCore workerCore)
        {
            SetupLogicPhase<EarlyPhaseId>(workerCore, OnEarlyPhase, "Logic Ecs Execution");
            SetupLogicPhase<DefaultPhaseId>(workerCore, OnDefaultPhase, "Logic Ecs Execution");
            SetupLogicPhase<LatePhaseId>(workerCore, OnLatePhase, "Logic Ecs Execution");

            SetupRenderPhase<RootPhaseId>(workerCore, OnRenderPhase, "Render Ecs Execution");

            return this;

            static void SetupLogicPhase<T0>(WorkerCycleCore workerCore, Action callback, string name) where T0 : IPhaseId
            {
                workerCore.FindFirstLogicPhaseAndModify<T0>((ref WorkerPhase phase) => 
                {
                    WorkerPhase[] subPhases = phase.GetPhases();
                    WorkerPhase ecsPhase = new WorkerPhase(EcsPhaseId.Default, name);
                    ecsPhase.SetCallback(callback);
                    Array.Resize(ref subPhases, subPhases.Length + 1);
                    subPhases[^1] = ecsPhase;
                    phase.SetPhases(subPhases);
                });
            }

            static void SetupRenderPhase<T0>(WorkerCycleCore workerCore, Action callback, string name) where T0 : IPhaseId
            {
                workerCore.FindFirstRenderPhaseAndModify<T0>((ref WorkerPhase phase) =>
                {
                    WorkerPhase[] subPhases = phase.GetPhases();
                    WorkerPhase ecsPhase = new WorkerPhase(EcsPhaseId.Default, name);
                    ecsPhase.SetCallback(callback);
                    Array.Resize(ref subPhases, subPhases.Length + 1);
                    subPhases[^1] = ecsPhase;
                    phase.SetPhases(subPhases);
                });
            }
        }

        public EcsRunner CreateSystemsAuto(EntityWorld world)
        {
            IEnumerable<Type> systemTypes = GetAllSystemTypes();
            foreach (var systemType in systemTypes)
            {
                if (Activator.CreateInstance(systemType) is BaseSystem system)
                    SetupSystem(system, world);
                else
                    DevConsole.Log("Error initializing system " + systemType.Name);
            }

            return this;

            static IEnumerable<Type> GetAllSystemTypes()
            {
                return typeof(EcsRunner).Assembly.GetTypes().Where(x => x.BaseType == typeof(BaseSystem));
            }
        }

        private void SetupSystem(BaseSystem system, EntityWorld world)
        {
            system.SetWorld(world);

            SystemGroupAttribute att = system.GetType().GetCustomAttribute<SystemGroupAttribute>();
            if (att == null)
                defaultSystems.Add(system);
            else 
            {
                if (att.PhaseType == PhaseType.Logic)
                {
                    if (att.GroupType == typeof(EarlyPhaseId))
                        earlySystems.Add(system);
                    else if (att.GroupType == typeof(DefaultPhaseId))
                        defaultSystems.Add(system);
                    else if (att.GroupType == typeof(LatePhaseId))
                        lateSystems.Add(system);
                }
                else if (att.PhaseType == PhaseType.Render)
                    renderSystems.Add(system);
                else if (att.PhaseType == PhaseType.Physics)
                    throw new NotImplementedException();
            }
        }

        private void OnEarlyPhase()
        {
            for (int i = 0; i < earlySystems.Count; i++)
                earlySystems[i].PerformWork();
        }

        private void OnDefaultPhase()
        {
            for (int i = 0; i < defaultSystems.Count; i++)
                defaultSystems[i].PerformWork();
        }

        private void OnLatePhase()
        {
            for (int i = 0; i < lateSystems.Count; i++)
                lateSystems[i].PerformWork();
        }

        private void OnRenderPhase()
        {
            for (int i = 0; i < renderSystems.Count; i++)
                renderSystems[i].PerformWork();
        }
    }
}

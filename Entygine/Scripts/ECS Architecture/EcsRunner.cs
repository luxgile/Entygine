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
            SetupLogicPhase<MainPhases.EarlyPhaseId>(workerCore, OnEarlyPhase, "Logic Ecs Execution");
            SetupLogicPhase<MainPhases.DefaultPhaseId>(workerCore, OnDefaultPhase, "Logic Ecs Execution");
            SetupLogicPhase<MainPhases.LatePhaseId>(workerCore, OnLatePhase, "Logic Ecs Execution");

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
                    if (att.GroupType == typeof(MainPhases.EarlyPhaseId))
                        AddSystem(ref earlySystems, system);
                    else if (att.GroupType == typeof(MainPhases.DefaultPhaseId))
                        AddSystem(ref defaultSystems, system);
                    else if (att.GroupType == typeof(MainPhases.LatePhaseId))
                        AddSystem(ref lateSystems, system);
                }
                else if (att.PhaseType == PhaseType.Render)
                    renderSystems.Add(system);
                else if (att.PhaseType == PhaseType.Physics)
                    throw new NotImplementedException();
            }
        }

        private void AddSystem(ref List<BaseSystem> systems, BaseSystem system)
        {
            var afterAtt = system.GetType().GetCustomAttribute<AfterSystemAttribute>();
            if (afterAtt != null)
            {
                int index = FindSystem(ref systems, afterAtt.SystemType);
                if (index != -1)
                {
                    if (index >= systems.Count)
                        systems.Add(system);
                    else
                        systems.Insert(index + 1, system);

                    return;
                }
            }

            var beforeAtt = system.GetType().GetCustomAttribute<BeforeSystemAttribute>();
            if (beforeAtt != null)
            {
                int index = FindSystem(ref systems, beforeAtt.SystemType);
                if (index != -1)
                {
                    systems.Insert(index, system);
                    return;
                }
            }

            systems.Add(system);

            static int FindSystem(ref List<BaseSystem> systems, Type systemType)
            {
                for (int i = 0; i < systems.Count; i++)
                {
                    if (systems[i].GetType() == systemType)
                        return i;
                }
                return -1;
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

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
        private List<BaseSystem> earlySystems = new();
        private List<BaseSystem> defaultSystems = new();
        private List<BaseSystem> lateSystems = new();
        private List<BaseSystem> renderSystems = new();

        public BaseSystem[] GetSystems<T0>() where T0 : IPhaseId
        {
            Type t0 = typeof(T0);
            if (t0 == typeof(MainPhases.EarlyPhaseId))
                return earlySystems.ToArray();
            if (t0 == typeof(MainPhases.DefaultPhaseId))
                return defaultSystems.ToArray();
            if (t0 == typeof(MainPhases.LatePhaseId))
                return lateSystems.ToArray();
            return null;
        }

        public EcsRunner AssignToWorker(WorkerCycleCore workerCore)
        {
            SetupLogicPhase<MainPhases.EarlyPhaseId>(workerCore, OnEarlyPhase, "Logic Ecs Execution");
            SetupLogicPhase<MainPhases.DefaultPhaseId>(workerCore, OnDefaultPhase, "Logic Ecs Execution");
            SetupLogicPhase<MainPhases.LatePhaseId>(workerCore, OnLatePhase, "Logic Ecs Execution");

            SetupRenderPhase<RootPhaseId>(workerCore, OnRenderPhase, "Render Ecs Execution");

            return this;

            static void SetupLogicPhase<T0>(WorkerCycleCore workerCore, Action<float> callback, string name) where T0 : IPhaseId
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

            static void SetupRenderPhase<T0>(WorkerCycleCore workerCore, Action<float> callback, string name) where T0 : IPhaseId
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
                    DevConsole.Log(LogType.Error, "Error initializing system " + systemType.Name);
            }

            return this;

            static IEnumerable<Type> GetAllSystemTypes()
            {
                return typeof(EcsRunner).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(BaseSystem)) && !x.IsAbstract);
            }
        }

        private void SetupSystem(BaseSystem system, EntityWorld world)
        {
            system.SetWorld(world);

            SystemGroupAttribute att = system.GetType().GetCustomAttribute<SystemGroupAttribute>();
            if (att == null)
                AddSystem(ref defaultSystems, system);
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
                    AddSystem(ref renderSystems, system);
            }
        }

        private void AddSystem(ref List<BaseSystem> systems, BaseSystem system)
        {
            systems.Add(system);
            SortSystems(ref systems);
        }

        private void SortSystems(ref List<BaseSystem> systems)
        {
            HashSet<BaseSystem> sortedSystems = new HashSet<BaseSystem>();
            HashSet<BaseSystem> loopDetectionHash = new HashSet<BaseSystem>();
            for (int i = 0; i < systems.Count; i++)
            {
                loopDetectionHash.Clear();
                SortAfterSystem(ref systems, systems[i]);

                loopDetectionHash.Clear();
                SortBeforeSystem(ref systems, systems[i]);
            }

            void SortAfterSystem(ref List<BaseSystem> systems, BaseSystem system)
            {
                if (sortedSystems.Contains(system))
                    return;

                var att = system.GetType().GetCustomAttribute<AfterSystemAttribute>();
                if (att == null)
                    return;

                int index = FindSystem(ref systems, att.SystemType);
                if (index == -1)
                    return;

                if(loopDetectionHash.Contains(system))
                {
                    DevConsole.Log(LogType.Warning, "Loop found sorting systems.");
                    return;
                }

                loopDetectionHash.Add(system);

                SortAfterSystem(ref systems, systems[index]);

                systems.Remove(system);
                if (index + 1 >= systems.Count)
                    systems.Add(system);
                else
                    systems.Insert(index + 1, system);

                sortedSystems.Add(system);
            }

            void SortBeforeSystem(ref List<BaseSystem> systems, BaseSystem system)
            {
                if (sortedSystems.Contains(system))
                    return;

                var att = system.GetType().GetCustomAttribute<BeforeSystemAttribute>();
                if (att == null)
                    return;

                int index = FindSystem(ref systems, att.SystemType);
                if (index == -1)
                    return;

                if (loopDetectionHash.Contains(system))
                {
                    DevConsole.Log(LogType.Warning, "Loop found sorting systems.");
                    return;
                }

                loopDetectionHash.Add(system);

                SortBeforeSystem(ref systems, systems[index]);

                systems.Remove(system);
                systems.Insert(index, system);

                sortedSystems.Add(system);
            }

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

        private void OnEarlyPhase(float dt)
        {
            for (int i = 0; i < earlySystems.Count; i++)
                earlySystems[i].PerformWork(dt);
        }

        private void OnDefaultPhase(float dt)
        {
            for (int i = 0; i < defaultSystems.Count; i++)
                defaultSystems[i].PerformWork(dt);
        }

        private void OnLatePhase(float dt)
        {
            for (int i = 0; i < lateSystems.Count; i++)
                lateSystems[i].PerformWork(dt);
        }

        private void OnRenderPhase(float dt)
        {
            for (int i = 0; i < renderSystems.Count; i++)
                renderSystems[i].PerformWork(dt);
        }
    }
}

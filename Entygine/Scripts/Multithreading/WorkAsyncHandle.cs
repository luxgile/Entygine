using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entygine.Async
{
    public class WorkAsyncHandle
    {
        public string Name { get; set; }
        private Task task;
        private List<WorkAsyncHandle> dependencies = new();
        private bool started;

        public WorkAsyncHandle() { }
        public WorkAsyncHandle(Action action)
        {
            task = new Task(action);
        }
        public WorkAsyncHandle(Action action, params WorkAsyncHandle[] dependencies)
        {
            task = new Task(action);
            this.dependencies = new (dependencies);
        }

        public void SetDependencies(params WorkAsyncHandle[] dependencies)
        {
            if (IsRunning)
                throw new Exception("Async Work is already running and can't be modified.");

            this.dependencies = new(dependencies);
        }

        public void AddDependencies(params WorkAsyncHandle[] dependencies)
        {
            if (IsRunning)
                throw new Exception("Async Work is already running and can't be modified.");

            this.dependencies.AddRange(dependencies);
        }

        public void SetAction(Action action)
        {
            if (IsRunning)
                throw new Exception("Async Work is already running and can't be modified.");

            task = new Task(action);
        }

        public void RunSync()
        {
            if (dependencies != null)
                dependencies.FinishAll();

            task.RunSynchronously();
        }

        public void Start()
        {
            started = true;
            if (dependencies != null && dependencies.Count > 0)
            {
                Task.WhenAll(dependencies.Select(x => x.Task)).ContinueWith((x) =>
                {
                    task.Start();
                });

                return;
            }

            task.Start();
        }

        public void FinishWork()
        {
            dependencies?.FinishAll();

            task.Wait();
        }

        public Task Task => task;
        public TaskStatus CurrentStatus => task.Status;
        public bool IsFinished => CurrentStatus is TaskStatus.RanToCompletion or TaskStatus.Faulted || started;
        public bool IsRunning => CurrentStatus is TaskStatus.Running or TaskStatus.WaitingToRun;
    }

    public static class WorkAsyncHandleExtensions
    {
        public static void FinishAll(this IEnumerable<WorkAsyncHandle> dependencies)
        {
            foreach (WorkAsyncHandle handle in dependencies)
                handle.FinishWork();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Entygine.Async
{
    public class WorkAsyncHandle
    {
        private readonly Task task;
        private readonly WorkAsyncHandle[] dependencies;

        public WorkAsyncHandle(Action action)
        {
            task = new Task(action);
        }
        public WorkAsyncHandle(Action action, params WorkAsyncHandle[] dependencies)
        {
            task = new Task(action);
            this.dependencies = dependencies;
        }

        public void RunSync()
        {
            if (dependencies != null)
                dependencies.FinishAll();

            task.RunSynchronously();
        }

        public void Start()
        {
            if (dependencies != null && dependencies.Length > 0)
            {
                dependencies.WhenAllFinish(this);
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
        public bool IsFinished => CurrentStatus is TaskStatus.RanToCompletion or TaskStatus.Faulted;
        public bool IsRunning => CurrentStatus is TaskStatus.Running or TaskStatus.WaitingToRun;
    }

    public static class WorkAsyncHandleExtensions
    {
        public static void FinishAll(this IEnumerable<WorkAsyncHandle> dependencies)
        {
            foreach (WorkAsyncHandle handle in dependencies)
                handle.FinishWork();
        }

        public static void WhenAllFinish(this IEnumerable<WorkAsyncHandle> dependencies, WorkAsyncHandle handle)
        {
            Task.WhenAll(dependencies.Select(x => x.Task)).ContinueWith((x) =>
            {
                handle.Task.Start();
            });
        }
    }
}

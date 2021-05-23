using System;
using System.Threading.Tasks;

namespace Entygine.Async
{
    public class WorkAsyncHandle
    {
        private Task task;
        private WorkAsyncHandle dependency;

        public WorkAsyncHandle(Action action)
        {
            task = new Task(action);
        }
        public WorkAsyncHandle(Action action, WorkAsyncHandle dependency)
        {
            task = new Task(action);
            this.dependency = dependency;
        }

        public void Sync()
        {
            if (dependency != null && dependency.CurrentStatus == TaskStatus.Running)
                dependency.FinishWork();

            task.RunSynchronously();
        }

        public void Async()
        {
            if (dependency != null && !dependency.IsFinished)
            {
                dependency.task.ContinueWith((x) => 
                { 
                    task.Start(); 
                });

                return;
            }

            task.Start();
        }

        public void FinishWork()
        {
            dependency?.FinishWork();

            task.Wait();
        }

        public TaskStatus CurrentStatus => task.Status;
        public bool IsFinished => CurrentStatus is TaskStatus.RanToCompletion or TaskStatus.Faulted;
        public bool IsRunning => CurrentStatus is TaskStatus.Running or TaskStatus.WaitingToRun;
    }
}

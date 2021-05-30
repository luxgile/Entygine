using Entygine.Async;

namespace Entygine.Ecs
{
    public abstract class QuerySystem : BaseSystem
    {
        protected EntityIterator Iterator { get; private set; }

        protected virtual bool CheckChanges { get; } = true;
        protected virtual bool RunAsync { get; } = true;

        protected override void OnSystemCreated()
        {
            base.OnSystemCreated();

            Iterator = new();
            Iterator.SetWorld(World);
        }

        protected sealed override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            OnFrame(dt);

            Iterator.SetVersion(CheckChanges ? LastVersionWorked : 0);
            WorkAsyncHandle workHandle = World.DependencyManager.InsertDependencies(Iterator.Settings.Descriptor, Iterator.Handle, RunAsync);
            if (RunAsync)
                workHandle.Start();
            else
                workHandle.RunSync();
        }

        protected abstract void OnFrame(float dt);
    }
}

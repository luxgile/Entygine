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

            Iterator.SetVersion(CheckChanges ? LastVersionWorked : 0);

            //TODO: Move dependency gathering before so the user itself runs the iteration and it's more intuitive.
            OnFrame(dt);

            //WorkAsyncHandle workHandle = World.DependencyManager.InsertDependencies(Iterator.Settings.Descriptor, Iterator.Handle, RunAsync);
            World.DependencyManager.InsertDependencies(Iterator.Settings.Descriptor, Iterator.Handle);

            if (RunAsync)
                Iterator.RunAsync();
            else
                Iterator.RunSync();
        }

        protected abstract void OnFrame(float dt);
    }
}

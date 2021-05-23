namespace Entygine.Ecs
{
    public abstract class QuerySystem<T> : BaseSystem where T : IIteratorPhase2, new()
    {
        protected T Iterator { get; private set; }

        protected virtual bool CheckChanges { get; } = true;

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
            //TODO: Create dependency tree
            Iterator.SetVersion(CheckChanges ? LastVersionWorked : 0).Synchronous();
        }

        protected abstract void OnFrame(float dt);
    }
}

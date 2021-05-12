namespace Entygine.Ecs
{
    public abstract class QuerySystem : BaseSystem
    {
        private QueryScope query;

        protected virtual bool CheckChanges { get; } = true;

        protected abstract QueryScope SetupQuery();

        protected override void OnSystemCreated()
        {
            base.OnSystemCreated();

            query = SetupQuery();
        }

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.OnlyChanged(CheckChanges ? LastVersionWorked : 0).Perform();
        }
    }
}

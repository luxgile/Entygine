namespace Entygine.Ecs
{
    public abstract class QuerySystem : BaseSystem
    {
        private QueryScope query;

        protected abstract QueryScope SetupQuery();

        protected override void OnSystemCreated()
        {
            base.OnSystemCreated();

            query = SetupQuery();
        }

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.OnlyChanged(LastVersionWorked).Perform();
        }
    }
}

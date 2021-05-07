namespace Entygine.Ecs
{
    public abstract class BaseSystem
    {
        private EntityWorld world;
        private bool started;
        private uint lastVersionWorked;

        public void SetWorld(EntityWorld world)
        {
            this.world = world;
        }

        ~BaseSystem()
        {
            OnSystemDestroyed();
        }

        public void PerformWork(float dt)
        {
            if (!started)
            {
                started = true;
                OnSystemCreated();
            }

            World.EntityManager.Version++;

            OnPerformFrame(dt);

            lastVersionWorked = World.EntityManager.Version;
        }

        protected virtual void OnSystemCreated() { }
        protected virtual void OnPerformFrame(float dt) { }
        protected virtual void OnSystemDestroyed() { }

        public EntityWorld World => world;
        public uint LastVersionWorked => lastVersionWorked;
    }
}

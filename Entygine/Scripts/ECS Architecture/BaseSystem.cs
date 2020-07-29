namespace Entygine.Ecs
{
    public class BaseSystem
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

        public void PerformWork()
        {
            if(!started)
            {
                started = true;
                OnSystemCreated();
            }

            World.EntityManager.Version++;

            OnPerformFrame();

            lastVersionWorked = World.EntityManager.Version;
        }

        protected virtual void OnSystemCreated() { }
        protected virtual void OnPerformFrame() { }
        protected virtual void OnSystemDestroyed() { }

        public EntityWorld World => world;
        public uint LastVersionWorked => lastVersionWorked;
    }
}

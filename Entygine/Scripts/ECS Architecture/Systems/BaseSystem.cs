using Entygine.DevTools;

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

            try
            {
                OnPerformFrame(dt);
            }
            catch(System.Exception e)
            {
                DevConsole.Log(LogType.Error, e.Message);
            }

            lastVersionWorked = World.EntityManager.Version;
        }

        protected virtual void OnSystemCreated() { }
        protected virtual void OnPerformFrame(float dt) { }
        protected virtual void OnSystemDestroyed() { }

        public ref T0 GetSingleton<T0>(TypeId id) where T0 : struct, ISingletonComponent
        {
            return ref World.EntityManager.GetSingleton<T0>(id);
        }

        public EntityWorld World => world;
        public uint LastVersionWorked => lastVersionWorked;
    }
}

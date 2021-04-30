using Entygine.Ecs;

namespace Entygine_Editor
{
    public abstract class ComponentDrawer : RawDrawer
    {
        protected object Context { get; private set; }
        public void SetComponentContext(IComponent component) => Context = component;
        public abstract int MatchesObject(object obj);
    }

    public abstract class ComponentDrawer<T0> : ComponentDrawer
    {
        protected new T0 Context => (T0)base.Context;
    }
}


namespace Entygine_Editor
{
    public abstract class DetailsDrawer : RawDrawer
    {
        protected object Context { get; private set; }

        public void SetContext(object obj)
        {
            Context = obj;
        }

        public sealed override bool Draw()
        {
            OnDraw();
            return true;
        }

        protected virtual void OnDraw() { }

        public abstract int MatchesObject(object obj);
    }

    public abstract class DetailsDrawer<T0> : DetailsDrawer
    {
        protected new T0 Context => (T0)base.Context;
    }
}

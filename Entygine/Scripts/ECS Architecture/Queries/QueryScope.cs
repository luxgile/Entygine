using System;

namespace Entygine.Ecs
{
    public delegate void QueryDelegate<T0>(ref T0 context);
    public interface IQueryContext { }
    public abstract class QueryScope
    {
        protected uint ChangeVersion { get; private set; } = 0;
        protected QuerySettings Settings { get; private set; }
        protected QueryScope(QuerySettings settings)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public QueryScope OnlyChanged(uint version)
        {
            ChangeVersion = version;
            return this;
        }

        public abstract void Perform();
    }
    public abstract class QueryScope<T0> : QueryScope where T0 : struct, IQueryContext
    {
        protected QueryDelegate<T0> Iterator { get; private set; }

        protected QueryScope(QuerySettings settings, QueryDelegate<T0> iterator) : base(settings)
        {
            Iterator = iterator ?? throw new ArgumentNullException(nameof(iterator));
        }
    }
}

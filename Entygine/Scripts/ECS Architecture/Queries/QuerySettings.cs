using Entygine.DevTools;
using System;

namespace Entygine.Ecs
{
    public interface IQueryIterator { }
    public interface IQueryEntityIterator : IQueryIterator { void Iteration(ref EntityChunk chunk, int index); }
    public interface IQueryChunkIterator : IQueryIterator { void Iteration(ref EntityChunk chunk); }

    public struct QueryDesc
    {
        public bool needsAny;
        public TypeId[] readWith;
        public TypeId[] writeWith;
        public TypeId[] readAny;
        public TypeId[] writeAny;
        public TypeId[] noneTypes;
    }

    public class QuerySettings
    {
        private QueryDesc desc;

        public QuerySettings RWith(params TypeId[] types)
        {
            desc.readWith = types;
            return this;
        }

        public QuerySettings WWith(params TypeId[] types)
        {
            desc.writeWith = types;
            return this;
        }

        public QuerySettings RAny(params TypeId[] types)
        {
            desc.readAny = types;
            return this;
        }
        public QuerySettings WAny(params TypeId[] types)
        {
            desc.writeAny = types;
            return this;
        }
        public QuerySettings None(params TypeId[] types)
        {
            desc.noneTypes = types;
            return this;
        }

        /// <summary>
        /// Marks if at least one any needs to be found in order to match.
        /// </summary>
        public void NeedsAny(bool state) => desc.needsAny = state;

        public bool Matches(EntityArchetype archetype)
        {
            bool withCheck = true;
            if (desc.readWith != null && desc.readWith.Length > 0)
                withCheck = archetype.HasTypes(desc.readWith);
            if (desc.writeWith != null && desc.writeWith.Length > 0)
                withCheck &= archetype.HasTypes(desc.writeWith);

            bool noneCheck = true;
            if (desc.noneTypes != null && desc.noneTypes.Length > 0)
                noneCheck = !archetype.HasAnyTypes(desc.noneTypes);

            bool anyCheck = !desc.needsAny;
            if (desc.readAny != null && desc.readAny.Length > 0)
                anyCheck |= archetype.HasAnyTypes(desc.readAny);
            if (desc.writeAny != null && desc.writeAny.Length > 0)
                anyCheck |= archetype.HasAnyTypes(desc.writeAny);

            return withCheck && noneCheck && anyCheck;
        }

        public QueryDesc Descriptor => desc;

        public static readonly QuerySettings Empty = new QuerySettings();
    }
}

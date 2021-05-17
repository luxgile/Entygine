using Entygine.DevTools;
using System;

namespace Entygine.Ecs
{
    public interface IQueryIterator { }
    public interface IQueryEntityIterator : IQueryIterator { void Iteration(ref EntityChunk chunk, int index); }
    public interface IQueryChunkIterator : IQueryIterator { void Iteration(ref EntityChunk chunk); }

    public class QuerySettings
    {
        private TypeId[] withTypes;
        private TypeId[] anyTypes;
        private TypeId[] noneTypes;

        public QuerySettings With(params TypeId[] types)
        {
            this.withTypes = types;
            return this;
        }

        public QuerySettings Any(params TypeId[] types)
        {
            this.anyTypes = types;
            return this;
        }

        public QuerySettings None(params TypeId[] types)
        {
            this.noneTypes = types;
            return this;
        }

        public bool Matches(EntityArchetype archetype)
        {
            bool withCheck = true;
            if (withTypes != null && withTypes.Length > 0)
                withCheck = archetype.HasTypes(withTypes);

            bool noneCheck = true;
            if (noneTypes != null && noneTypes.Length > 0)
                noneCheck = !archetype.HasAnyTypes(noneTypes);

            bool anyCheck = true;
            if (anyTypes != null && anyTypes.Length > 0)
                anyCheck = archetype.HasAnyTypes(anyTypes);

            return withCheck && noneCheck && anyCheck;
        }

        public TypeId[] WithTypes => withTypes;
        public TypeId[] AnyTypes => anyTypes;

        //public bool IsGeneralWrite()
        //{
        //    if (withTypes == null)
        //        return false;

        //    for (int i = 0; i < withTypes.Length; i++)
        //    {
        //        if (!withTypes[i].IsReadOnly)
        //            return true;
        //    }

        //    return false;
        //}

        //public bool NeedsUpdate(ref EntityChunk chunk)
        //{
        //    if (anyTypes == null)
        //        return false;

        //    for (int i = 0; i < anyTypes.Length; i++)
        //    {
        //        TypeId type = anyTypes[i];
        //        if (chunk.Archetype.HasTypes(type) && !type.IsReadOnly)
        //            return true;
        //    }
        //    return false;
        //}

        public static readonly QuerySettings Empty = new QuerySettings();
    }
}

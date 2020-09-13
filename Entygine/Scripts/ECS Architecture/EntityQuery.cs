using Entygine.DevTools;
using System;

namespace Entygine.Ecs
{
    public interface IQueryIterator { }
    public interface IQueryEntityIterator : IQueryIterator { void Iteration(ref EntityChunk chunk, int index); }
    public interface IQueryChunkIterator : IQueryIterator { void Iteration(ref EntityChunk chunk); }

    public class EntityQuery
    {
        private TypeCache[] withTypes;
        private TypeCache[] anyTypes;

        public EntityQuery With(params TypeCache[] types)
        {
            this.withTypes = types;
            return this;
        }

        public EntityQuery Any(params TypeCache[] types)
        {
            this.anyTypes = types;
            return this;
        }

        //TODO: Iteration problably should be somewhere else.

        public bool Matches(EntityArchetype archetype)
        {
            bool withCheck = true;
            if (withTypes != null)
                withCheck = archetype.HasTypes(withTypes);

            bool anyCheck = true;
            if (anyTypes != null)
                anyCheck = archetype.HasAnyTypes(anyTypes);

            return withCheck && anyCheck;
        }

        public bool IsGeneralWrite()
        {
            if (withTypes == null)
                return false;

            for (int i = 0; i < withTypes.Length; i++)
            {
                if (!withTypes[i].IsReadOnly)
                    return true;
            }

            return false;
        }

        public bool NeedsUpdate(ref EntityChunk chunk)
        {
            if (anyTypes == null)
                return false;

            for (int i = 0; i < anyTypes.Length; i++)
            {
                TypeCache type = anyTypes[i];
                if (chunk.Archetype.HasTypes(type) && !type.IsReadOnly)
                    return true;
            }
            return false;
        }
    }
}

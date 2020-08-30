using Entygine.DevTools;
using System;

namespace Entygine.Ecs
{
    public interface IQueryIterator { }
    public interface IQueryEntityIterator : IQueryIterator { void Iteration(ref EntityChunk chunk, int index); }
    public interface IQueryChunkIterator : IQueryIterator { void Iteration(ref EntityChunk chunk); }

    public class EntityQuery
    {
        private EntityWorld world;
        private TypeCache[] withTypes;
        private TypeCache[] anyTypes;

        public EntityQuery(EntityWorld world)
        {
            this.world = world;
        }

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

        public void Perform(IQueryIterator iterator) => Perform(iterator, false, 0);
        public void Perform(IQueryIterator iterator, uint version) => Perform(iterator, true, version);
        private void Perform(IQueryIterator iterator, bool checkVersion, uint version)
        {
            switch (iterator)
            {
                default:
                DevConsole.Log("Query doesn't have an iterator.");
                return;

                case IQueryChunkIterator chunkIterator:
                PerformChunkyIteration(chunkIterator, checkVersion, version);
                return;

                case IQueryEntityIterator entityIterator:
                PerformEntityIteration(entityIterator, checkVersion, version);
                return;
            }
        }

        private void PerformChunkyIteration(IQueryChunkIterator chunkIterator, bool checkVersion, uint version)
        {
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
                if (chunk.Archetype.HasTypes(withTypes))
                {
                    if (checkVersion && !chunk.HasChanged(version))
                        continue;

                    chunkIterator.Iteration(ref chunk);

                    //TODO: Update chunk version only if write
                    chunk.ChunkVersion = world.EntityManager.Version;
                }
            }
        }

        private void PerformEntityIteration(IQueryEntityIterator entityIterator, bool checkVersion, uint version)
        {
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            bool generalWrite = IsGeneralWrite();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
                if (ChunkMatch(ref chunk))
                {
                    for (int c = 0; c < chunk.Count; c++)
                    {
                        if (checkVersion && !chunk.HasChanged(version))
                            continue;

                        entityIterator.Iteration(ref chunk, c);

                        //TODO: Update chunk version only if write
                        if (generalWrite || NeedsUpdate(ref chunk))
                            chunk.ChunkVersion = world.EntityManager.Version;
                    }
                }
            }
        }

        private bool ChunkMatch(ref EntityChunk chunk)
        {
            bool withCheck = true;
            if (withTypes != null)
                withCheck = chunk.Archetype.HasTypes(withTypes);

            bool anyCheck = true;
            if (anyTypes != null)
                anyCheck = chunk.Archetype.HasAnyTypes(anyTypes);

            return withCheck && anyCheck;
        }

        private bool IsGeneralWrite()
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

        private bool NeedsUpdate(ref EntityChunk chunk)
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

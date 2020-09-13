using Entygine.DevTools;

namespace Entygine.Ecs
{
    public class EntityIterator
    {
        public static void PerformIteration(EntityWorld world, IQueryIterator iterator, EntityQuery query) => Perform(world, iterator, query, false, 0);
        public static void PerformIteration(EntityWorld world, IQueryIterator iterator, EntityQuery query, uint version) => Perform(world, iterator, query, true, version);
        private static void Perform(EntityWorld world, IQueryIterator iterator, EntityQuery query, bool checkVersion, uint version)
        {
            switch (iterator)
            {
                default:
                    DevConsole.Log("Iterator not found.");
                    return;

                case IQueryChunkIterator chunkIterator:
                    PerformChunkIteration(world, chunkIterator, query, checkVersion, version);
                    return;

                case IQueryEntityIterator entityIterator:
                    PerformEntityIteration(world, entityIterator, query, checkVersion, version);
                    return;
            }
        }

        private static void PerformChunkIteration(EntityWorld world, IQueryChunkIterator chunkIterator, EntityQuery query, bool checkVersion, uint version)
        {
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
                if (query.Matches(chunk.Archetype))
                {
                    if (checkVersion && !chunk.HasChanged(version))
                        continue;

                    chunkIterator.Iteration(ref chunk);

                    //TODO: Update chunk version only if write
                    chunk.UpdateVersion(world.EntityManager.Version);
                }
            }
        }

        private static void PerformEntityIteration(EntityWorld world, IQueryEntityIterator entityIterator, EntityQuery query, bool checkVersion, uint version)
        {
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            bool generalWrite = query.IsGeneralWrite();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
                if (query.Matches(chunk.Archetype))
                {
                    for (int c = 0; c < chunk.Count; c++)
                    {
                        if (checkVersion && !chunk.HasChanged(version))
                            continue;

                        entityIterator.Iteration(ref chunk, c);

                        //TODO: Update chunk version only if write
                        //VERSION IS NOT UPDATED CORRECTLY
                        if (generalWrite || query.NeedsUpdate(ref chunk))
                            chunk.UpdateVersion(world.EntityManager.Version);
                    }
                }
            }
        }
    }
}

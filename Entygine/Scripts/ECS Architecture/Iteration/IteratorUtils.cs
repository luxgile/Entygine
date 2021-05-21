using System;
using System.Collections.Generic;

namespace Entygine.Ecs
{
    public static class IteratorUtils
    {
        public static IteratorAction ForEachChunk(EntityWorld world, QuerySettings settings, uint version, Action<EntityChunk> act)
        {
            return () =>
            {
                List<EntityChunk> chunks = new();
                world.EntityManager.GetChunks(settings, chunks);
                for (int i = 0; i < chunks.Count; i++)
                {
                    EntityChunk chunk = chunks[i];

                    if (!chunk.HasChanged(version))
                        continue;

                    act(chunk);

                    chunk.UpdateVersion(world.EntityManager.Version);
                }
            };
        }
    }
}

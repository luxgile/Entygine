using Entygine.Async;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public static WorkAsyncHandle ForEachChunkAsync(EntityWorld world, QuerySettings settings, uint version, Action<EntityChunk> act, params WorkAsyncHandle[] dependencies)
        {
            WorkAsyncHandle forEach = new (() =>
            {
                List<EntityChunk> chunks = new();
                List<Task> tasks = new();
                world.EntityManager.GetChunks(settings, chunks);
                for (int i = 0; i < chunks.Count; i++)
                {
                    EntityChunk chunk = chunks[i];

                    if (!chunk.HasChanged(version))
                        continue;

                    var handle = new WorkAsyncHandle(() => act(chunk));
                    handle.Start();
                    tasks.Add(handle.Task);

                    chunk.UpdateVersion(world.EntityManager.Version);
                }
                Task.WaitAll(tasks.ToArray());
            }, dependencies);
            forEach.Start();
            return forEach;
        }
    }
}

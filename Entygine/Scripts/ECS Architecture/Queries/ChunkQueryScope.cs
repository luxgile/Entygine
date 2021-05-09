using System;
using System.Collections.Generic;

namespace Entygine.Ecs
{
    public class ChunkQueryScope : QueryScope<ChunkQueryContext>
    {
        public ChunkQueryScope(QuerySettings settings, QueryDelegate<ChunkQueryContext> iterator) : base(settings, iterator) { }

        public override void Perform()
        {
            EntityWorld world = EntityWorld.Active;
            bool generalWrite = Settings.IsGeneralWrite();
            List<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (!Settings.Matches(chunk.Archetype))
                    continue;

                if (!chunk.HasChanged(ChangeVersion))
                    continue;

                ChunkQueryContext context = new(chunk);
                Iterator(context);

                if (generalWrite && Settings.NeedsUpdate(ref chunk))
                    chunk.UpdateVersion(world.EntityManager.Version);
            }
        }
    }

    public struct ChunkQueryContext : IQueryContext
    {
        public EntityChunk Chunk { private get; init; }

        public ChunkQueryContext(EntityChunk chunk)
        {
            Chunk = chunk;
        }

        public void ReadComponent<T0>(int index, out T0 component) where T0 : IComponent
        {
            Chunk.TryGetComponent(index, out component);
        }

        public void WriteComponent<T0>(int index, T0 component) where T0 : IComponent
        {
            Chunk.SetComponent(index, component);
        }

        public void Read<T0>(out T0 shared) where T0 : ISharedComponent
        {
            Chunk.TryGetSharedComponent(out shared);
        }

        public void Write<T0>(T0 shared) where T0 : ISharedComponent
        {
            Chunk.SetSharedComponent(shared);
        }

        public int GetEntityCount() => Chunk.Count;
    }
}

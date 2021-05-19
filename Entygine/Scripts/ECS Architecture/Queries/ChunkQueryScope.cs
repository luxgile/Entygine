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
            List<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (!Settings.Matches(chunk.Archetype))
                    continue;

                if (!chunk.HasChanged(ChangeVersion))
                    continue;

                ChunkQueryContext context = new(chunk);
                Iterator(ref context);

                if (context.HasWriten)
                    chunk.UpdateVersion(world.EntityManager.Version);
            }
        }
    }

    public struct ChunkQueryContext : IQueryContext
    {
        public EntityChunk Chunk { private get; init; }
        public bool HasWriten { get; private set; }

        public ChunkQueryContext(EntityChunk chunk)
        {
            Chunk = chunk;
            HasWriten = false;
        }

        public void ReadComponent<T0>(int index, TypeId id, out T0 component) where T0 : IComponent
        {
            Chunk.TryGetComponent(index, id, out component);
        }

        public void WriteComponent<T0>(int index, TypeId id, T0 component) where T0 : IComponent
        {
            Chunk.SetComponent(index, id, component);
            HasWriten = true;
        }

        public void Read<T0>(TypeId id, out T0 shared) where T0 : ISharedComponent
        {
            Chunk.TryGetSharedComponent(id, out shared);
        }

        public void Write<T0>(TypeId id, T0 shared) where T0 : ISharedComponent
        {
            Chunk.SetSharedComponent(id, shared);
            HasWriten = true;
        }

        public int GetEntityCount() => Chunk.Count;
    }
}

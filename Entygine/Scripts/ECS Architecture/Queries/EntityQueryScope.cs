using System.Collections.Generic;

namespace Entygine.Ecs
{
    public class EntityQueryScope : QueryScope<EntityQueryContext>
    {
        private EntityWorld world;

        public EntityQueryScope(QuerySettings settings, QueryDelegate<EntityQueryContext> iterator) : base(settings, iterator) 
        {
            world = EntityWorld.Active;
        }

        public EntityQueryScope(QuerySettings settings, EntityWorld world, QueryDelegate<EntityQueryContext> iterator) : base(settings, iterator)
        {
            this.world = world;
        }

        public override void Perform()
        {
            bool generalWrite = Settings.IsGeneralWrite();
            List<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (!Settings.Matches(chunk.Archetype))
                    continue;

                if (!chunk.HasChanged(ChangeVersion))
                    continue;

                for (int e = 0; e < chunk.Count; e++)
                {
                    EntityQueryContext context = new(chunk, e);
                    Iterator(context);
                }

                if (generalWrite && Settings.NeedsUpdate(ref chunk))
                    chunk.UpdateVersion(world.EntityManager.Version);
            }
        }
    }

    public struct EntityQueryContext : IQueryContext
    {
        public EntityChunk Chunk { get; init; }
        public int Index { get; init; }

        public EntityQueryContext(EntityChunk chunk, int index)
        {
            this.Chunk = chunk;
            this.Index = index;
        }

        public void GetEntity(out Entity entity)
        {
            entity = Chunk.GetEntity(Index);
        }

        public bool Read<T0>(out T0 comp) where T0 : IComponent
        {
            return Chunk.TryGetComponent(Index, out comp);
        }

        public void ReadAll(out IComponent[] components)
        {
            Chunk.GetComponentsFromIndex(Index, out components);
        }

        public void Write<T0>(T0 comp) where T0 : IComponent
        {
            Chunk.SetComponent(Index, comp);
        }
    }
}

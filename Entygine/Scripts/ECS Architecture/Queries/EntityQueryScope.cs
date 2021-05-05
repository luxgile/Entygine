namespace Entygine.Ecs
{
    public class EntityQueryScope : QueryScope<EntityQueryContext>
    {
        public EntityQueryScope(QuerySettings settings, QueryDelegate<EntityQueryContext> iterator) : base(settings, iterator) { }

        public override void Perform()
        {
            EntityWorld world = EntityWorld.Active;
            bool generalWrite = Settings.IsGeneralWrite();
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
                if (!Settings.Matches(chunk.Archetype))
                    continue;

                if (!chunk.HasChanged(ChangeVersion))
                    continue;

                for (int e = 0; e < chunk.Count; e++)
                {
                    EntityQueryContext context = new(world, i, e);
                    Iterator(context);
                }

                if (generalWrite && Settings.NeedsUpdate(ref chunk))
                    chunk.UpdateVersion(world.EntityManager.Version);
            }
        }
    }

    public struct EntityQueryContext : IQueryContext
    {
        public EntityWorld World { get; init; }
        public int Chunk { get; init; }
        public int Index { get; init; }

        public EntityQueryContext(EntityWorld world, int chunk, int index)
        {
            this.World = world;
            this.Chunk = chunk;
            this.Index = index;
        }

        public bool Read<T0>(out T0 comp) where T0 : IComponent
        {
            ref EntityChunk ec = ref World.EntityManager.GetChunk(Chunk);
            return ec.TryGetComponent(Index, out comp);
        }

        public void Write<T0>(T0 comp) where T0 : IComponent
        {
            ref EntityChunk ec = ref World.EntityManager.GetChunk(Chunk);
            ec.SetComponent(Index, comp);
        }
    }
}

namespace Entygine.Ecs
{
    public class ChunkQueryScope : QueryScope<ChunkQueryContext>
    {
        public ChunkQueryScope(QuerySettings settings, QueryDelegate<ChunkQueryContext> iterator) : base(settings, iterator) { }

        public override void Perform()
        {

        }
    }

    public struct ChunkQueryContext : IQueryContext
    {
        public EntityWorld World { get; init; }
        public int Chunk { get; init; }

        public void ReadComponent<T0>(int index, out T0 component) where T0 : IComponent
        {
            ref EntityChunk chunk = ref World.EntityManager.GetChunk(Chunk);
            chunk.TryGetComponent(index, out component);
        }

        public void WriteComponent<T0>(int index, T0 component) where T0 : IComponent
        {
            ref EntityChunk chunk = ref World.EntityManager.GetChunk(Chunk);
            chunk.SetComponent(index, component);
        }

        public void Read<T0>(out T0 shared) where T0 : ISharedComponent
        {
            ref EntityChunk chunk = ref World.EntityManager.GetChunk(Chunk);
            chunk.TryGetSharedComponent(out shared);
        }

        public void Write<T0>(T0 shared) where T0 : ISharedComponent
        {
            ref EntityChunk chunk = ref World.EntityManager.GetChunk(Chunk);
            chunk.SetSharedComponent(shared);
        }

        public int GetEntityCount() => World.EntityManager.GetChunk(Chunk).Count;
    }
}

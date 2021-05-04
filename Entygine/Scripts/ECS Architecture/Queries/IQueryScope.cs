using System;

namespace Entygine.Ecs
{
    public delegate void QueryDelegate<T0>(T0 context);
    public interface IQueryContext { }
    public abstract class QueryScope<T0> where T0 : struct, IQueryContext
    {
        protected QueryDelegate<T0> Iterator { get; private set; }
        protected EntityQuerySettings Settings { get; private set; }
        protected uint ChangeVersion { get; private set; } = 0;

        public QueryScope<T0> Setup(EntityQuerySettings settings)
        {
            Settings = settings;
            return this;
        }
        public QueryScope<T0> Iteration(QueryDelegate<T0> iterator)
        {
            Iterator = iterator;
            return this;
        }
        public QueryScope<T0> OnlyChanged(uint version)
        {
            ChangeVersion = version;
            return this;
        }
        public abstract void Perform();
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

        public void Read<T0>(out T0 comp) where T0 : IComponent
        {
            ref EntityChunk ec = ref World.EntityManager.GetChunk(Chunk);
            ec.TryGetComponent(Index, out comp);
        }

        public void Write<T0>(T0 comp) where T0 : IComponent
        {
            ref EntityChunk ec = ref World.EntityManager.GetChunk(Chunk);
            ec.SetComponent(Index, comp);
        }
    }

    public class EntityQueryScope : QueryScope<EntityQueryContext>
    {
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
}

using System.Collections.Generic;
using System.Runtime.CompilerServices;

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
            if (world == null)
                world = EntityWorld.Active;

            List<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (!Settings.Matches(chunk.Archetype))
                    continue;

                if (!chunk.HasChanged(ChangeVersion))
                    continue;

                bool hasWriten = false;
                for (int e = 0; e < chunk.Count; e++)
                {
                    EntityQueryContext context = new(chunk, e);
                    Iterator(ref context);
                    hasWriten = context.HasWriten;
                }

                if (hasWriten)
                    chunk.UpdateVersion(world.EntityManager.Version);
            }
        }

        public delegate void R<R1>(ref R1 read) where R1 : IComponent;
        public unsafe void Iterate<R1>(R<R1> readIteration) where R1 : struct, IComponent
        {
            TypeId readId = TypeManager.GetIdFromType(typeof(R1));
            List<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (!Settings.Matches(chunk.Archetype))
                    continue;

                if (!chunk.HasChanged(ChangeVersion))
                    continue;

                chunk.TryGetComponents(readId, out ComponentArray collection);
                for (int e = 0; e < chunk.Count; e++)
                {
                    IComponent cm = collection[e];
                    ref R1 rcr = ref Unsafe.Unbox<R1>(cm);
                    readIteration(ref rcr);
                }

                chunk.UpdateVersion(world.EntityManager.Version);
            }
        }
    }

    public struct EntityQueryContext : IQueryContext
    {
        public EntityChunk Chunk { get; init; }
        public int Index { get; init; }
        public bool HasWriten { get; private set; }

        public EntityQueryContext(EntityChunk chunk, int index)
        {
            this.Chunk = chunk;
            this.Index = index;
            HasWriten = false;
        }

        public void GetEntity(out Entity entity)
        {
            entity = Chunk.GetEntity(Index);
        }

        public bool Read<T0>(TypeId id, out T0 comp) where T0 : IComponent
        {
            return Chunk.TryGetComponent(Index, id, out comp);
        }

        public void ReadAll(out TypeId[] ids, out IComponent[] components)
        {
            Chunk.GetComponentsFromIndex(Index, out ids, out components);
        }

        public void Write<T0>(TypeId id, T0 comp) where T0 : IComponent
        {
            HasWriten = true;
            Chunk.SetComponent(Index, id, comp);
        }
    }
}

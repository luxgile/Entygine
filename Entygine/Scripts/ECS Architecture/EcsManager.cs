using System;
using System.Collections.Generic;

namespace Entygine.Ecs
{
    public class EntityManager
    {
        private uint version;
        private StructArray<EntityChunk> chunks;

        public EntityManager()
        {
            chunks = new StructArray<EntityChunk>();
        }

        public List<int> GetChunksWith(EntityArchetype archetype, bool readOnly)
        {
            List<int> chunksFound = new List<int>();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk currChunk = ref chunks[i];
                if (currChunk.HasArchetype(archetype))
                {
                    chunksFound.Add(i);
                    if (!readOnly)
                    {
                        //TODO: Make somehow you can't change a chunk if it's on Read Only
                        currChunk.ChunkVersion = version;
                    }
                }
            }
            return chunksFound;
        }

        public ref EntityChunk GetChunk(int index) => ref chunks[index];

        public void SetComponent<T0>(Entity entity, T0 component) where T0 : IComponent
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
                for (int e = 0; e < chunk.Count; e++)
                {
                    Entity currEntity = chunk.GetEntity(e);
                    if (currEntity.id == entity.id)
                    {
                        chunk.SetComponent(e, component);
                        return;
                    }
                }
            }

            throw new Exception("Entity not found.");
        }

        public void SetSharedComponent<T0>(Entity entity, T0 component) where T0 : ISharedComponent
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                int index = i;
                ref EntityChunk chunk = ref chunks[index];
                for (int e = 0; e < chunk.Count; e++)
                {
                    Entity currEntity = chunk.GetEntity(e);
                    if (currEntity.id == entity.id)
                    {
                        if (chunk.HasSharedComponents(component))
                            return;

                        if (chunk.IsSharedComponentEmpty<T0>())
                            chunk.SetSharedComponent(component);
                        else
                        {
                            //Create a new chunk with the given archetype and new shared component
                            chunk.GetComponentsFromEntity(entity, out List<IComponent> components);
                            chunk.DestroyEntity(entity);

                            //TODO: Check if a better chunk is avaliable instead of creating one always
                            int newChunkIndex = CreateChunk(chunk.Archetype);
                            ref EntityChunk newChunk = ref chunks[newChunkIndex];
                            newChunk.AddEntity(entity);
                            newChunk.SetSharedComponents(chunk.GetSharedComponents());
                            newChunk.SetSharedComponent(component);
                            newChunk.SetComponents(entity, components.ToArray());
                        }

                        return;
                    }
                }
            }

            throw new Exception("Entity not found.");
        }

        public int GetEntityCount()
        {
            int count = 0;
            for (int i = 0; i < chunks.Count; i++)
                count += chunks[i].Count;
            return count;
        }

        public Entity CreateEntity(EntityArchetype archetype)
        {
            uint id = (uint)GetEntityCount() + 1;
            int chunkIndex = GetAvaliableChunk(archetype);
            ref EntityChunk chunk = ref chunks[chunkIndex];
            chunk.ChunkVersion = version;
            Entity entity = chunk.CreateEntity(id);
            return entity;
        }

        /// <summary>
        /// Gets the first chunk that matchs the archetype and it's not full.
        /// </summary>
        private int GetAvaliableChunk(EntityArchetype archetype)
        {
            List<int> chunksFound = GetChunksWith(archetype, true);
            if (chunksFound.Count == 0)
                return CreateChunk(archetype);
            else
            {
                //We assume the rest of chunks are full since to create a new one requires the last one to be full.
                int lastChunk = chunksFound[chunksFound.Count - 1];
                if (chunks[lastChunk].IsFull)
                    return CreateChunk(archetype);
                else
                    return lastChunk;
            }
        }

        private int CreateChunk(EntityArchetype archetype)
        {
            EntityChunk chunk = new EntityChunk(archetype)
            {
                ChunkVersion = version
            };

            int index = -1;
            bool added = false;
            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].HasArchetype(archetype))
                {
                    chunks.Insert(i + 1, chunk);
                    index = i + 1;
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                index = chunks.Count;
                chunks.Add(chunk);
            }

            return index;
        }

        public StructArray<EntityChunk> GetChunks() => chunks;

        public uint Version { get => version; internal set => version = value; }
    }
}

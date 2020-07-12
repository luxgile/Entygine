using System;
using System.Collections.Generic;

namespace Entygine.Ecs
{
    public class EntityManager
    {
        private uint version;
        private List<EntityChunk> chunks;

        public EntityManager()
        {
            chunks = new List<EntityChunk>();
        }

        public List<EntityChunk> GetChunksWith(EntityArchetype archetype, bool readOnly)
        {
            List<EntityChunk> chunksFound = new List<EntityChunk>();
            for (int i = 0; i < chunks.Count; i++)
            {
                var currChunk = chunks[i];
                if (currChunk.HasArchetype(archetype))
                {
                    chunksFound.Add(currChunk);
                    if (!readOnly)
                    {
                        //TODO: Make somehow you can't change a chunk if it's on Read Only
                        currChunk.ChunkVersion = version;
                    }
                }
            }
            return chunksFound;
        }

        public void SetComponent<T0>(Entity entity, T0 component) where T0 : IComponent
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                for (int e = 0; e < chunk.Count; e++)
                {
                    Entity currEntity = chunk.GetEntity(e);
                    if (currEntity.id == entity.id)
                    {
                        if (chunk.TryGetComponents<T0>(out ComponentArray comp))
                            comp[e] = component;
                        else
                            throw new Exception("Entity found but doesn't have the expected component.");

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
                EntityChunk chunk = chunks[i];
                for (int e = 0; e < chunk.Count; e++)
                {
                    Entity currEntity = chunk.GetEntity(e);
                    if (currEntity.id == entity.id)
                    {
                        if (chunk.HasSharedComponent(component))
                            chunk.SetSharedComponent(component);
                        else
                        {
                            //Create a new chunk with the given archetype and new shared component
                            chunk.DestroyEntity(entity);

                            EntityChunk newChunk = CreateChunk(chunk.Archetype);
                            newChunk.CreateEntity(entity.id);
                            newChunk.SetSharedComponent(component);
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
            var chunk = GetAvaliableChunk(archetype);
            chunk.ChunkVersion = version;
            Entity entity = chunk.CreateEntity(id);
            return entity;
        }

        /// <summary>
        /// Gets the first chunk that matchs the archetype and it's not full.
        /// </summary>
        private EntityChunk GetAvaliableChunk(EntityArchetype archetype)
        {
            var chunksFound = GetChunksWith(archetype, true);
            if (chunksFound.Count == 0)
                return CreateChunk(archetype);
            else
            {
                //We assume the rest of chunks are full since to create a new one requires the last one to be full.
                var lastChunk = chunksFound[chunksFound.Count - 1];
                if (lastChunk.IsFull)
                    return CreateChunk(archetype);
                else
                    return lastChunk;
            }
        }

        private EntityChunk CreateChunk(EntityArchetype archetype)
        {
            EntityChunk chunk = new EntityChunk(archetype);
            chunk.ChunkVersion = version;
            bool added = false;
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk currChunk = chunks[i];
                if (currChunk.HasArchetype(archetype))
                {
                    chunks.Insert(i + 1, chunk);
                    added = true;
                    break;
                }
            }

            if (!added)
                chunks.Add(chunk);

            return chunk;
        }

        public uint Version { get => version; internal set => version = value; }
    }
}

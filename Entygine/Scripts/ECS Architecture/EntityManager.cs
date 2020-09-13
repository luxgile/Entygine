using NUnit.Framework;
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
                        chunk.UpdateVersion(version);
                        return;
                    }
                }
            }

            throw new Exception("Entity not found.");
        }

        public void SetSharedComponent<T0>(Entity entity, T0 component) where T0 : ISharedComponent
        {
            int chunkFound = -1;
            for (int i = 0; i < chunks.Count; i++)
            {
                int index = i;
                ref EntityChunk chunk = ref chunks[index];
                if (chunk.HasEntity(entity))
                {
                    //Already has the component.
                    if (chunk.HasSharedComponents(component))
                        return;

                    //Needs to be initialized
                    if (chunk.IsSharedComponentEmpty<T0>())
                    {
                        chunk.SetSharedComponent(component);
                        return;
                    }

                    //Entity can't stay in this chunk so we stop and find a valid chunk.
                    chunkFound = index;
                    break;
                }
            }

            if(chunkFound == -1)
                throw new Exception("Entity not found.");

            ref EntityChunk fromChunk = ref chunks[chunkFound];
            ISharedComponent[] sharedsToCopy = fromChunk.GetSharedComponents();
            ISharedComponent[] sharedsToFind = new ISharedComponent[sharedsToCopy.Length];
            Array.Copy(sharedsToCopy, sharedsToFind, sharedsToCopy.Length);
            for (int i = 0; i < sharedsToFind.Length; i++)
            {
                if (sharedsToFind[i] is T0)
                    sharedsToFind[i] = component;
            }

            for (int i = 0; i < chunks.Count; i++)
            {
                int index = i;
                ref EntityChunk chunk = ref chunks[index];
                if(!chunk.IsFull && chunk.HasArchetype(fromChunk.Archetype) && (chunk.HasSharedComponents(sharedsToFind)))
                {
                    //Valid chunk found. Move entity to it.
                    fromChunk.GetComponentsFromEntity(entity, out List<IComponent> components);
                    fromChunk.DestroyEntity(entity);

                    chunk.AddEntity(entity);
                    chunk.SetSharedComponent(component);
                    chunk.SetComponents(entity, components.ToArray());
                    return;
                }
            }

            {
                //Create a new chunk with the given archetype and new shared component
                fromChunk.GetComponentsFromEntity(entity, out List<IComponent> components);
                fromChunk.DestroyEntity(entity);

                int newChunkIndex = CreateChunk(fromChunk.Archetype);
                ref EntityChunk newChunk = ref chunks[newChunkIndex];
                newChunk.AddEntity(entity);
                newChunk.SetSharedComponents(fromChunk.GetSharedComponents());
                newChunk.SetSharedComponent(component);
                newChunk.SetComponents(entity, components.ToArray());
            }
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
            chunk.UpdateVersion(version);
            Entity entity = chunk.CreateEntity(id);
            return entity;
        }

        /// <summary>
        /// Gets the first chunk that matchs the archetype and it's not full.
        /// </summary>
        private int GetAvaliableChunk(EntityArchetype archetype)
        {
            List<int> chunksFound = GetChunksWith(archetype);
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

        private List<int> GetChunksWith(EntityArchetype archetype)
        {
            List<int> chunksFound = new List<int>();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk currChunk = ref chunks[i];
                if (currChunk.HasArchetype(archetype))
                    chunksFound.Add(i);
            }
            return chunksFound;
        }

        private int CreateChunk(EntityArchetype archetype)
        {
            EntityChunk chunk = new EntityChunk(archetype);
            chunk.UpdateVersion(version);

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

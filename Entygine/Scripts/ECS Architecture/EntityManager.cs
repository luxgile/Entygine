using Entygine.Mathematics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public EntityChunk GetChunk(int index) => chunks[index];

        public void SetComponent<T0>(Entity entity, TypeId id, T0 component) where T0 : IComponent
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                for (int e = 0; e < chunk.Count; e++)
                {
                    Entity currEntity = chunk.GetEntity(e);
                    if (currEntity.id == entity.id)
                    {
                        chunk.SetComponent(e, id, component);
                        chunk.UpdateVersion(version);
                        return;
                    }
                }
            }

            throw new Exception("Entity not found.");
        }

        public void SetSharedComponent<T0>(Entity entity, TypeId id, T0 component) where T0 : ISharedComponent
        {
            int chunkFound = -1;
            for (int i = 0; i < chunks.Count; i++)
            {
                int index = i;
                EntityChunk chunk = chunks[index];
                if (chunk.HasEntity(entity))
                {
                    //Already has the component.
                    if (chunk.HasSharedComponents(component))
                        return;

                    //Needs to be initialized
                    if (chunk.IsSharedComponentEmpty<T0>())
                    {
                        chunk.SetSharedComponent(id, component);
                        return;
                    }

                    //Entity can't stay in this chunk so we stop and find a valid chunk.
                    chunkFound = index;
                    break;
                }
            }

            if (chunkFound == -1)
                throw new Exception("Entity not found.");

            EntityChunk fromChunk = chunks[chunkFound];
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
                EntityChunk chunk = chunks[index];
                if (!chunk.IsFull && chunk.HasArchetype(fromChunk.Archetype) && (chunk.HasSharedComponents(sharedsToFind)))
                {
                    //Valid chunk found. Move entity to it.
                    fromChunk.GetComponentsFromEntity(entity, out TypeId[] ids, out IComponent[] components);
                    fromChunk.DestroyEntity(entity);

                    chunk.AddEntity(entity);
                    chunk.SetSharedComponent(id, component);
                    chunk.SetComponents(entity, ids, components);
                    return;
                }
            }

            {
                //Create a new chunk with the given archetype and new shared component
                fromChunk.GetComponentsFromEntity(entity, out TypeId[] ids, out IComponent[] components);
                fromChunk.DestroyEntity(entity);

                int newChunkIndex = CreateChunk(fromChunk.Archetype);
                EntityChunk newChunk = chunks[newChunkIndex];
                newChunk.AddEntity(entity);
                newChunk.SetSharedComponents(fromChunk.Archetype.GetSharedTypes(), fromChunk.GetSharedComponents());
                newChunk.SetSharedComponent(id, component);
                newChunk.SetComponents(entity, ids, components);
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
            EntityChunk chunk = chunks[chunkIndex];
            chunk.UpdateVersion(version);
            Entity entity = chunk.CreateEntity(id);
            return entity;
        }

        public Entity[] CreateEntities(EntityArchetype archetype, uint count)
        {
            if (count == 0)
                return null;

            List<Entity> entities = new List<Entity>();
            uint startId = (uint)GetEntityCount() + 1;
            while (count > 0)
            {
                EntityChunk avalChunk = chunks[GetAvaliableChunk(archetype)];
                uint toFill = MathUtils.Min(count, (uint)avalChunk.CountLeft);
                avalChunk.UpdateVersion(version);
                avalChunk.CreateEntities(startId, toFill, ref entities);
                startId += toFill;
                count -= toFill;
            }
            return entities.ToArray();
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
                int lastChunk = chunksFound[^1];
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
                EntityChunk currChunk = chunks[i];
                if (currChunk.HasArchetype(archetype))
                    chunksFound.Add(i);
            }
            return chunksFound;
        }

        private int CreateChunk(EntityArchetype archetype)
        {
            EntityChunk chunk = new EntityChunk(archetype);
            chunk.UpdateVersion(version);
            int prevChunk = -1;
            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].HasArchetype(archetype))
                    prevChunk = i;
                //We have surparsed the last chunk with the archetype.
                else if (prevChunk != -1)
                    break;
            }

            int index;
            if (prevChunk != -1)
            {
                chunks.Insert(prevChunk + 1, chunk);
                index = prevChunk + 1;
            }
            else
            {
                index = chunks.Count;
                chunks.Add(chunk);
            }

            return index;
        }

        public void GetChunks(QuerySettings settings, List<EntityChunk> foundChunks)
        {
            foundChunks.Clear();
            for (int i = 0; i < chunks.Count; i++)
            {
                EntityChunk chunk = chunks[i];
                if (settings.Matches(chunk.Archetype))
                    foundChunks.Add(chunk);
            }
        }

        public List<EntityChunk> GetChunks() => chunks;

        public uint Version { get => version; internal set => version = value; }
    }
}

using System;
using System.Runtime.InteropServices;

namespace Entygine.Ecs
{
    public class EntityChunk
    {
        private const int CHUNK_SIZE = 16000;

        private Entity[] entities;
        private ComponentArray[] componentCollections;
        private ISharedComponent[] sharedComponents;
        private EntityArchetype arch;
        private int currentCount;
        private uint chunkVersion;

        public EntityChunk(EntityArchetype arch)
        {
            unsafe
            {
                currentCount = 0;
                this.arch = arch;

                Type[] cTypes = arch.GetComponenTypes();
                Type[] sTypes = arch.GetSharedTypes();
                int entityAmount = arch.GetChunkCapacity(CHUNK_SIZE);

                entities = new Entity[entityAmount];
                for (int i = 0; i < entityAmount; i++)
                    entities[i].id = 0;

                componentCollections = new ComponentArray[cTypes.Length];
                for (int i = 0; i < componentCollections.Length; i++)
                    componentCollections[i] = new ComponentArray(cTypes[i], entityAmount);

                sharedComponents = new ISharedComponent[sTypes.Length];
            }
        }

        public Entity CreateEntity(uint id)
        {
            if (IsFull)
                throw new Exception("Chunk is full. Create a new one.");

            int index = Count;
            entities[index].id = id;
            currentCount++;
            return entities[index];
        }

        public void DestroyEntity(Entity entity)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                Entity currEntity = entities[i];
                if(currEntity.id == entity.id)
                {
                    entities[i].id = 0;
                    currentCount--;
                    break;
                }
            }
        }

        public Entity GetEntity(int index)
        {
            return entities[index];
        }

        public bool HasChanged(uint version)
        {
            if (chunkVersion == 0)
                return true;

            return version < chunkVersion;
        }

        public bool TryGetComponents<T0>(out ComponentArray comp) where T0 : IComponent
        {
            for (int i = 0; i < componentCollections.Length; i++)
            {
                var currCollection = componentCollections[i];
                if (currCollection.TypeMatch<T0>())
                {
                    comp = currCollection;
                    return true;
                }
            }

            comp = null;
            return false;
        }

        public bool TryGetSharedComponents<T0>(out T0 comp) where T0 : ISharedComponent
        {
            for (int i = 0; i < sharedComponents.Length; i++)
            {
                ISharedComponent sComp = sharedComponents[i];
                if (sComp is T0 temp)
                {
                    comp = temp;
                    return true;
                }
            }

            comp = default;
            return false;
        }

        public bool HasArchetype(EntityArchetype archetype)
        {
            return arch.TypeMatch(archetype);
        }

        public bool MatchSharedComponent(params ISharedComponent[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                bool found = false;
                var comp = components[i];
                for (int t = 0; t < sharedComponents.Length; t++)
                {
                    var sharedComp = sharedComponents[t];
                    if (sharedComp.GetHashCode() == comp.GetHashCode())
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns if the shared component is in the chunk or it's not initialized yet.
        /// </summary>
        public bool HasSharedComponent<T0>(T0 component) where T0 : ISharedComponent
        {
            if (!arch.HasSharedType(typeof(T0)))
                throw new Exception("Shared component not found in chunk.");

            bool empty = false;
            for (int i = 0; i < sharedComponents.Length; i++)
            {
                ISharedComponent sharedComp = sharedComponents[i];
                if (sharedComp == null)
                    empty = true;
                else if (sharedComp is T0 comp)
                    return comp.GetHashCode() == component.GetHashCode();
            }

            return empty;
        }

        public void SetSharedComponent<T0>(T0 component) where T0 : ISharedComponent
        {
            if (!arch.HasSharedType(typeof(T0)))
                throw new Exception("Shared component not found in chunk.");

            int indexNull = -1;
            for (int i = 0; i < sharedComponents.Length; i++)
            {
                ISharedComponent sharedComp = sharedComponents[i];
                if (sharedComp == null && indexNull == -1)
                    indexNull = i;
                else if (sharedComp is T0 comp)
                {
                    sharedComponents[i] = component;
                    return;
                }
            }

            if (indexNull != -1)
                sharedComponents[indexNull] = component;
            else
                throw new Exception("Shared component not found in chunk.");
        }

        public bool IsFull => Count == Capacity;
        public int Count => currentCount;
        public int Capacity => entities.Length;
        public EntityArchetype Archetype => arch;
        public uint ChunkVersion 
        { 
            get => chunkVersion; 
            internal set => chunkVersion = value; 
        }
    }
}

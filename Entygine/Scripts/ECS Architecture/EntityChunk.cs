using System;

namespace Entygine.Ecs
{
    //TODO: Change this shit to a class, it only makes everything more complicated.
    public class EntityChunk
    {
        private const int CHUNK_SIZE = 16000;

        private StructArray<Entity> entities;
        private ComponentArray[] componentCollections;
        private ISharedComponent[] sharedComponents;
        private EntityArchetype arch;
        private int currentCount;
        private uint chunkVersion;

        public EntityChunk(EntityArchetype arch)
        {
            unsafe
            {
                chunkVersion = 0;
                currentCount = 0;
                this.arch = arch;

                TypeCache[] cTypes = arch.GetComponenTypes();
                TypeCache[] sTypes = arch.GetSharedTypes();
                int entityAmount = arch.GetChunkCapacity(CHUNK_SIZE);

                entities = new StructArray<Entity>(entityAmount, true);
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
            entities[index].version++;
            currentCount++;
            return entities[index];
        }

        public void AddEntity(Entity entity)
        {
            if (IsFull)
                throw new Exception("Chunk is full. Create a new one.");

            int index = Count;
            entities[index] = entity;
            currentCount++;
        }

        public void DestroyEntity(Entity entity)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].id == entity.id)
                {
                    entities.SwapForLast(entity);

                    for (int c = 0; c < componentCollections.Length; c++)
                    {
                        ComponentArray collection = componentCollections[c];
                        collection[i] = collection[currentCount - 1];
                    }

                    currentCount--;
                    break;
                }
            }
        }

        public bool HasEntity(Entity entity)
        {
            for (int i = 0; i < Count; i++)
            {
                if (entities[i].Equals(entity))
                    return true;
            }

            return false;
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

        public void UpdateVersion(uint version)
        {
            chunkVersion = version;
        }

        public bool TryGetComponent<T0>(int index, out T0 component) where T0 : IComponent
        {
            if (TryGetComponents<T0>(out ComponentArray comp))
            {
                component = (T0)comp[index];
                return true;
            }

            component = default;
            return false;
        }

        public bool TryGetComponents<T0>(out ComponentArray comp) where T0 : IComponent
        {
            for (int i = 0; i < componentCollections.Length; i++)
            {
                ComponentArray currCollection = componentCollections[i];
                if (currCollection.TypeMatch<T0>())
                {
                    comp = currCollection;
                    return true;
                }
            }

            comp = null;
            return false;
        }

        public bool TryGetComponents(TypeCache componentType, out ComponentArray comp)
        {
            for (int i = 0; i < componentCollections.Length; i++)
            {
                ComponentArray currCollection = componentCollections[i];
                if (currCollection.TypeMatch(componentType))
                {
                    comp = currCollection;
                    return true;
                }
            }

            comp = null;
            return false;
        }

        public ISharedComponent[] GetSharedComponents() => sharedComponents;

        private int GetEntityIndex(Entity entity)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entity.Equals(entities[i]))
                    return i;
            }

            throw new Exception("Entity not found.");
        }

        public void GetComponentsFromEntity(Entity entity, out IComponent[] components)
        {
            int entityIndex = GetEntityIndex(entity);
            GetComponentsFromIndex(entityIndex, out components);
        }
        public void GetComponentsFromIndex(int index, out IComponent[] components)
        {
            components = new IComponent[componentCollections.Length];
            for (int c = 0; c < componentCollections.Length; c++)
                components[c] = (componentCollections[c][index]);
        }

        public void SetComponent<T0>(int index, T0 component) where T0 : IComponent
        {
            if (TryGetComponents<T0>(out ComponentArray comp))
                comp[index] = component;
            else
                throw new Exception("Entity doesn't have the expected component.");
        }

        public void SetComponents(Entity entity, params IComponent[] components)
        {
            int index = GetEntityIndex(entity);
            for (int i = 0; i < components.Length; i++)
            {
                IComponent currComponent = components[i];
                if (TryGetComponents(TypeCache.GetTypeCache(currComponent.GetType(), false), out ComponentArray componentArray))
                    componentArray[index] = currComponent;
            }
        }

        public bool TryGetSharedComponent<T0>(out T0 comp) where T0 : ISharedComponent
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

        public bool HasSharedComponents(params ISharedComponent[] components)
        {
            for (int i = 0; i < components.Length; i++)
            {
                bool found = false;
                ISharedComponent comp = components[i];
                if (comp == null)
                    continue;

                for (int t = 0; t < sharedComponents.Length; t++)
                {
                    var sharedComp = sharedComponents[t];
                    if (sharedComp == null)
                        continue;

                    if (sharedComp.Equals(comp))
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

        public bool IsSharedComponentEmpty<T0>() where T0 : ISharedComponent
        {
            for (int i = 0; i < sharedComponents.Length; i++)
            {
                ISharedComponent sharedComp = sharedComponents[i];
                if (sharedComp != null && sharedComp is T0)
                    return false;
            }

            return true;
        }

        public void SetSharedComponent<T0>(T0 component) where T0 : ISharedComponent
        {
            if (!arch.HasSharedType(TypeCache.GetTypeCache(typeof(T0), false)))
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

        public void SetSharedComponents(params ISharedComponent[] shareds)
        {
            for (int i = 0; i < shareds.Length; i++)
            {
                ISharedComponent currShared = shareds[i];
                if (currShared == null)
                    continue;

                if (!arch.HasSharedType(TypeCache.GetTypeCache(currShared.GetType(), false)))
                    throw new Exception("Shared component not found in chunk.");

                int indexNull = -1;
                for (int s = 0; s < sharedComponents.Length; s++)
                {
                    ISharedComponent sharedComp = sharedComponents[s];
                    if (sharedComp == null && indexNull == -1)
                        indexNull = s;
                    else if (sharedComp.GetType() == currShared.GetType())
                    {
                        sharedComponents[s] = currShared;
                        return;
                    }
                }

                if (indexNull != -1)
                    sharedComponents[indexNull] = currShared;
                else
                    throw new Exception("Shared component not found in chunk.");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is EntityChunk chunk 
                && chunk.Archetype.Equals(Archetype)
                && chunk.Count == Count
                && chunk.sharedComponents == sharedComponents;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Count, Archetype);
        }

        public bool IsFull => Count == Capacity;
        public int Count => currentCount;
        public int Capacity => entities.Count;
        public EntityArchetype Archetype => arch;
        public uint ChunkVersion
        {
            get => chunkVersion;
        }
    }
}

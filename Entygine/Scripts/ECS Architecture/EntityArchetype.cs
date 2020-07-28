using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Entygine.Ecs
{
    public class EntityArchetype
    {
        private Type[] componentTypes;
        private Type[] sharedComponentTypes;

        public EntityArchetype(params Type[] types)
        {
            if (types.Length == 0)
                throw new Exception("Cannot create an archetype with 0 types.");

            List<Type> cTypes = new List<Type>();
            List<Type> sTypes = new List<Type>();
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                bool isComponentType = typeof(IComponent).IsAssignableFrom(type);
                bool isSharedType = typeof(ISharedComponent).IsAssignableFrom(type);
                if(isComponentType && isSharedType)
                    throw new Exception($"Cannot use type {type} because it inherits from IComponent and ISharedComponent");
                if(!isComponentType && !isSharedType)
                    throw new Exception($"Cannot use type {type} because it doesn't inherit from any Component type");

                if (isComponentType)
                    cTypes.Add(type);
                else if (isSharedType)
                    sTypes.Add(type);
            }

            this.componentTypes = cTypes.ToArray();
            this.sharedComponentTypes = sTypes.ToArray();
        }

        public Type[] GetComponenTypes()
        {
            return componentTypes;
        }

        public Type[] GetSharedTypes()
        {
            return sharedComponentTypes;
        }

        public bool HasSharedType(Type type)
        {
            for (int i = 0; i < sharedComponentTypes.Length; i++)
            {
                if (sharedComponentTypes[i] == type)
                    return true;
            }

            return false;
        }

        public int GetChunkCapacity(int chunkSize)
        {
            unsafe
            {
                //TODO: Probably this needs to be debugged since i don't know if i'm missing any additional data
                int eSize = sizeof(Entity);

                Type[] cTypes = componentTypes;
                Type[] sTypes = sharedComponentTypes;

                int cSize = 0;
                for (int i = 0; i < cTypes.Length; i++)
                    cSize += Marshal.SizeOf(cTypes[i]);

                int sSize = 0;
                for (int i = 0; i < sTypes.Length; i++)
                    sSize += Marshal.SizeOf(sTypes[i]);

                int sizePerEntity = eSize + cSize;

                int entityAmount = (chunkSize - sSize) / sizePerEntity;
                return entityAmount;
            }
        }

        public bool TypeMatch(EntityArchetype archetype)
        {
            Type[] cTypes = archetype.componentTypes;
            for (int i = 0; i < cTypes.Length; i++)
            {
                bool found = false;
                var currType = cTypes[i];
                for (int t = 0; t < componentTypes.Length; t++)
                {
                    var myType = componentTypes[t];
                    if (myType == currType)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            Type[] sTypes = archetype.sharedComponentTypes;
            for (int i = 0; i < sTypes.Length; i++)
            {
                bool found = false;
                var currType = sTypes[i];
                for (int t = 0; t < sharedComponentTypes.Length; t++)
                {
                    var myType = sharedComponentTypes[t];
                    if (myType == currType)
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

        public override bool Equals(object obj)
        {
            return obj is EntityArchetype archetype &&
                   EqualityComparer<Type[]>.Default.Equals(componentTypes, archetype.componentTypes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(componentTypes);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < sharedComponentTypes.Length; i++)
                s += sharedComponentTypes[i].Name + ", ";

            for (int i = 0; i < componentTypes.Length; i++)
                s += componentTypes[i].Name + (i < componentTypes.Length - 1 ? ", " : "");

            return s;
        }
    }
}

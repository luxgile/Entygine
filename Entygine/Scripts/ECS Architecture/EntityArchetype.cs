using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace Entygine.Ecs
{
    public class EntityArchetype
    {
        private TypeId[] componentTypes;
        private TypeId[] sharedComponentTypes;

        private int cSize;
        private int sSize; 

        public EntityArchetype(params TypeId[] types)
        {
            if (types.Length == 0)
                throw new Exception("Cannot create an archetype with 0 types.");

            List<TypeId> cTypes = new List<TypeId>();
            List<TypeId> sTypes = new List<TypeId>();
            for (int i = 0; i < types.Length; i++)
            {
                TypeId typeId = types[i];
                Type type = TypeManager.GetTypeFromId(typeId);
                bool isComponentType = typeof(IComponent).IsAssignableFrom(type);
                bool isSharedType = typeof(ISharedComponent).IsAssignableFrom(type);
                if(isComponentType && isSharedType)
                    throw new Exception($"Cannot use type {typeId} because it inherits from IComponent and ISharedComponent");
                if(!isComponentType && !isSharedType)
                    throw new Exception($"Cannot use type {typeId} because it doesn't inherit from any Component type");



                if (isComponentType)
                {
                    cTypes.Add(typeId);

                    unsafe
                    {
                        cSize += Marshal.SizeOf(type);
                    }
                }
                else if (isSharedType)
                {
                    sTypes.Add(typeId);

                    unsafe
                    {
                        sSize += Marshal.SizeOf(type);
                    }
                }
            }

            this.componentTypes = cTypes.ToArray();
            this.sharedComponentTypes = sTypes.ToArray();
        }

        public TypeId[] GetComponenTypes()
        {
            return componentTypes;
        }

        public TypeId[] GetSharedTypes()
        {
            return sharedComponentTypes;
        }

        public bool HasSharedType(TypeId type)
        {
            for (int i = 0; i < sharedComponentTypes.Length; i++)
            {
                if (sharedComponentTypes[i] == type)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if all types are present in this archetype
        /// </summary>
        public bool HasTypes(params TypeId[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (!HasType(types[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if any type is present in this archetype
        /// </summary>
        public bool HasAnyTypes(params TypeId[] types)
        {
            if (types == null || types.Length == 0)
                return false;

            for (int i = 0; i < types.Length; i++)
            {
                if (HasType(types[i]))
                    return true;
            }

            return false;
        }

        private bool HasType(TypeId type)
        {
            if (HasSharedType(type))
                return true;

            for (int i = 0; i < componentTypes.Length; i++)
            {
                if (componentTypes[i] == type)
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

                int sizePerEntity = eSize + cSize;

                int entityAmount = (chunkSize - sSize) / sizePerEntity;
                return entityAmount;
            }
        }

        public bool TypeMatch(EntityArchetype archetype)
        {
            TypeId[] cTypes = archetype.componentTypes;
            for (int i = 0; i < cTypes.Length; i++)
            {
                bool found = false;
                var currType = cTypes[i];
                for (int t = 0; t < componentTypes.Length; t++)
                {
                    TypeId myType = componentTypes[t];
                    if (myType == currType)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                    return false;
            }

            TypeId[] sTypes = archetype.sharedComponentTypes;
            for (int i = 0; i < sTypes.Length; i++)
            {
                bool found = false;
                var currType = sTypes[i];
                for (int t = 0; t < sharedComponentTypes.Length; t++)
                {
                    TypeId myType = sharedComponentTypes[t];
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
                   EqualityComparer<TypeId[]>.Default.Equals(componentTypes, archetype.componentTypes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(componentTypes);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < sharedComponentTypes.Length; i++)
                s += sharedComponentTypes[i].Id + ", ";

            for (int i = 0; i < componentTypes.Length; i++)
                s += componentTypes[i].Id + (i < componentTypes.Length - 1 ? ", " : "");

            return s;
        }
    }
}

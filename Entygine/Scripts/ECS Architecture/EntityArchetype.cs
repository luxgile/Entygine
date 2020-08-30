using OpenToolkit.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Entygine.Ecs
{
    public class EntityArchetype
    {
        private TypeCache[] componentTypes;
        private TypeCache[] sharedComponentTypes;

        private int cSize;
        private int sSize; 

        public EntityArchetype(params Type[] types)
        {
            if (types.Length == 0)
                throw new Exception("Cannot create an archetype with 0 types.");

            List<TypeCache> cTypes = new List<TypeCache>();
            List<TypeCache> sTypes = new List<TypeCache>();
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
                    cTypes.Add(TypeCache.GetTypeCache(type, true));
                else if (isSharedType)
                    sTypes.Add(TypeCache.GetTypeCache(type, true));
            }

            CalculateChunkCapacity(cTypes, sTypes);

            this.componentTypes = cTypes.ToArray();
            this.sharedComponentTypes = sTypes.ToArray();
        }

        public TypeCache[] GetComponenTypes()
        {
            return componentTypes;
        }

        public TypeCache[] GetSharedTypes()
        {
            return sharedComponentTypes;
        }

        public bool HasSharedType(TypeCache type)
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
        public bool HasTypes(params TypeCache[] types)
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
        public bool HasAnyTypes(params TypeCache[] types)
        {
            for (int i = 0; i < types.Length; i++)
            {
                if (HasType(types[i]))
                    return true;
            }

            return false;
        }

        private bool HasType(TypeCache type)
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

        private void CalculateChunkCapacity(List<TypeCache> cTypes, List<TypeCache> sTypes)
        {
            unsafe
            {
                //TODO: Probably this needs to be debugged since i don't know if i'm missing any additional data
                cSize = 0;
                for (int i = 0; i < cTypes.Count; i++)
                    cSize += Marshal.SizeOf(cTypes[i].Type);

                sSize = 0;
                for (int i = 0; i < sTypes.Count; i++)
                    sSize += Marshal.SizeOf(sTypes[i].Type);
            }
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
            TypeCache[] cTypes = archetype.componentTypes;
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

            TypeCache[] sTypes = archetype.sharedComponentTypes;
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
                   EqualityComparer<TypeCache[]>.Default.Equals(componentTypes, archetype.componentTypes);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(componentTypes);
        }

        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < sharedComponentTypes.Length; i++)
                s += sharedComponentTypes[i].Type.Name + ", ";

            for (int i = 0; i < componentTypes.Length; i++)
                s += componentTypes[i].Type.Name + (i < componentTypes.Length - 1 ? ", " : "");

            return s;
        }
    }
}

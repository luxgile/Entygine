using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Entygine.Ecs
{
    public class ComponentArray
    {
        private TypeId typeId;
        private IComponent[] components;

        public ComponentArray(TypeId typeId, int count)
        {
            this.typeId = typeId;
            components = new IComponent[count];

            //TODO: This is awful but works for now
            for (int i = 0; i < count; i++)
                components[i] = (IComponent)Activator.CreateInstance(TypeManager.GetTypeFromId(typeId));
        }

        public IComponent this[int index]
        {
            get => components[index];
            set => components[index] = value;
        }

        public T0 Get<T0>(int index) where T0 : IComponent
        {
            return (T0)this[index];
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(components, array, index);
        }

        public bool TypeMatch(TypeId id)
        {
            return id == typeId;
        }

        public T0[] CastTo<T0>() where T0 : IComponent
        {
            return components.Cast<T0>().ToArray();
        }

        public int Count => components.Length;
    }
}

using System;
using System.Collections;
using System.Linq;

namespace Entygine.Ecs
{
    public class ComponentArray
    {
        private Type componentType;
        private IComponent[] components;

        public ComponentArray(Type componentType, int count)
        {
            this.componentType = componentType ?? throw new ArgumentNullException(nameof(componentType));
            components = new IComponent[count];

            //TODO: This is awful but works for now
            for (int i = 0; i < count; i++)
                components[i] = (IComponent)Activator.CreateInstance(componentType);
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

        public bool TypeMatch<T0>() where T0 : IComponent
        {
            return TypeMatch(typeof(T0));
        }
        public bool TypeMatch(Type type)
        {
            return type == this.componentType;
        }

        public T0[] CastTo<T0>() where T0 : IComponent
        {
            return components.Cast<T0>().ToArray();
        }

        public int Count => components.Length;
    }
}

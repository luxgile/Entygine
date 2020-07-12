using System;
using System.Collections;

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

        public ComponentArray<TComp> CastTo<TComp>() where TComp : IComponent
        {
            ComponentArray<TComp> comp = new ComponentArray<TComp>(Count);
            for (int i = 0; i < Count; i++)
                comp[i] = (TComp)this[i];
            return comp;
        }

        public int Count => components.Length;
    }

    public class ComponentArray<TComp> where TComp : IComponent
    {
        private TComp[] components;

        public ComponentArray(int count)
        {
            components = new TComp[count];

            //TODO: This is awful but works for now
            for (int i = 0; i < count; i++)
                components[i] = Activator.CreateInstance<TComp>();
        }

        public TComp this[int index]
        {
            get => components[index];
            set => components[index] = value;
        }

        public bool TypeMatch<T0>() where T0 : IComponent
        {
            return TypeMatch(typeof(T0));
        }
        public bool TypeMatch(Type type)
        {
            return type == typeof(TComp);
        }

        public int Count => components.Length;
    }
}

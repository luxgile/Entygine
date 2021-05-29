using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Entygine.Ecs
{
    public static class TypeManager
    {
        private static Type[] idToType = Array.Empty<Type>();
        private static Dictionary<Type, TypeId> typeToId = new();

        internal static void InitializeComponentsIdentifiers()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var allTypes = assemblies.SelectMany(x => x.GetTypes()).ToArray();
            Type[] types = allTypes
                .Where(t => t.GetInterfaces().Any(x => x == typeof(IComponent) || x == typeof(ISharedComponent) || x == typeof(ISingletonComponent)))
                .ToArray();
            
            int index = 0;
            idToType = new Type[types.Length];
            foreach (var type in types)
            {
                if (type == typeof(object))
                    continue;

                var field = type.GetProperty("Identifier", BindingFlags.Static | BindingFlags.Public);
                TypeId id = new(index);
                field.SetValue(null, id);
                idToType[index] = type;
                typeToId.Add(type, id);
                index++;
            }
        }

        public static Type GetTypeFromId(TypeId id)
        {
            return idToType[id.Id];
        }

        public static TypeId GetIdFromType(Type type)
        {
            typeToId.TryGetValue(type, out TypeId id);
            return id;
        }
    }
}

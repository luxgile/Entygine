using System;
using System.Collections.Generic;

namespace Entygine.Ecs
{
    public static partial class TypeManager
    {
        private static readonly Dictionary<TypeId, Type> idToType = new Dictionary<TypeId, Type>();

        public static Type GetTypeFromId(TypeId id)
        {
            if (idToType.TryGetValue(id, out Type type))
                return type;

            return null;
        }
    }
}

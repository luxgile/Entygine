using System;
using System.Collections.Generic;

namespace Entygine.Ecs
{
    [Obsolete("Use TypeId instead.")]
    public class TypeCache : IEquatable<TypeCache>
    {
        private static Dictionary<Type, TypeCache> collection = new Dictionary<Type, TypeCache>();

        public static TypeCache WriteType(Type type) => GetTypeCache(type, false);
        public static TypeCache WriteType<T>() => GetTypeCache(typeof(T), false);
        public static TypeCache ReadType(Type type) => GetTypeCache(type, true);
        public static TypeCache ReadType<T>() => GetTypeCache(typeof(T), true);
        public static TypeCache GetTypeCache(Type type, bool readOnly)
        {
            if (collection.TryGetValue(type, out TypeCache cache))
                cache.IsReadOnly = readOnly;
            else
            {
                cache = new TypeCache() { Type = type, IsReadOnly = readOnly };
                collection.Add(type, cache);
            }

            return cache;
        }

        public bool Equals(TypeCache other)
        {
            return other.Type == Type;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type);
        }

        public bool IsReadOnly { get; private set; }
        public Type Type { get; private set; }
    }
}

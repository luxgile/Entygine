using System;
using System.Collections.Generic;

namespace Entygine.Ecs
{
    public class TypeCache : IEquatable<TypeCache>
    {
        private static Dictionary<Guid, TypeCache> collection = new Dictionary<Guid, TypeCache>();
        private Guid hash;

        public static TypeCache WriteType(Type type) => GetTypeCache(type, false);
        public static TypeCache ReadType(Type type) => GetTypeCache(type, true);
        public static TypeCache ReadType<T>() => GetTypeCache(typeof(T), true);
        public static TypeCache GetTypeCache(Type type, bool readOnly)
        {
            Guid hash = type.GUID;
            if (collection.TryGetValue(hash, out TypeCache cache))
                cache.IsReadOnly = readOnly;
            else
            {
                cache = new TypeCache() { Type = type, hash = hash, IsReadOnly = readOnly };
                collection.Add(hash, cache);
            }

            return cache;
        }

        public bool Equals(TypeCache other)
        {
            return other.hash == hash;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(hash);
        }

        public bool IsReadOnly { get; private set; }
        public Type Type { get; private set; }
    }
}

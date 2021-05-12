using System;
using System.Runtime.CompilerServices;

namespace Entygine.Ecs
{
    public struct TypeId : IEquatable<TypeId>
    {
        public readonly int Id;

        public TypeId(int id)
        {
            Id = id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TypeId other)
        {
            return other.Id == Id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is TypeId id && Equals(id);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return Id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(TypeId left, TypeId right)
        {
            return left.Id == right.Id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(TypeId left, TypeId right)
        {
            return left.Id != right.Id;
        }
    }
}

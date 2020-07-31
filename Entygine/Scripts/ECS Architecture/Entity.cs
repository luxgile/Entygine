using System;

namespace Entygine.Ecs
{
    public struct Entity : IEquatable<Entity>
    {
        public uint id;
        public uint version;

        public bool IsValid => id != 0;
        public static Entity NullEntity => new Entity() { id = 0 };

        public bool Equals(Entity entity)
        {
            return id == entity.id && version == entity.version;
        }
        public override bool Equals(object obj)
        {
            return obj is Entity entity && Equals(entity);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(id, version);
        }

        public override string ToString()
        {
            return $"ID: {id} / Version: {version}";
        }
    }
}

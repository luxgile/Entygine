namespace Entygine.Ecs
{
    public struct Entity
    {
        public uint id;

        public bool IsValid => id != 0;
        public static Entity NullEntity => new Entity() { id = 0 };
    }
}

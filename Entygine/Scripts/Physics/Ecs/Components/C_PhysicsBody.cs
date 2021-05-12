using Entygine.Physics; 

namespace Entygine.Ecs
{
    public partial struct C_PhysicsBody : IComponent
    {
        public int id;
        public PhysicBody body;
    }
}

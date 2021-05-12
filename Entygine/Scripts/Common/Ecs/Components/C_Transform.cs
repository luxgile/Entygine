using OpenTK.Mathematics;

namespace Entygine.Ecs
{
    public partial struct C_Transform : IComponent
    {
        public Matrix4 value;
    }
}
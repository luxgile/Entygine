using OpenTK.Mathematics;

namespace Entygine.Ecs.Components
{
    public partial struct C_EditorCamera : IComponent
    {
        public float speed;
        public float sensitivity;
        public float focusDistance;
        public float yaw;
        public float pitch;
        public Vector3 focusPoint;
    }
}
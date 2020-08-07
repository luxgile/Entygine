using OpenToolkit.Mathematics;

namespace Entygine.Ecs.Components
{
    public struct C_EditorCamera : IComponent 
    {
        public float speed;

        //TODO: This should be done in a different system that converts Position => Matrix
        public Vector3 dir;
        public Vector3 pos;
    }
}

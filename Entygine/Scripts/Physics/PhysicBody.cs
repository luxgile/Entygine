using OpenToolkit.Mathematics;

namespace Entygine.Physics
{
    public class PhysicBody
    {
        public Vector3 position;
        public Quaternion rotation;

        public Vector3 velocity;
        public Quaternion torque;

        public void Step(float stepTime)
        {
            position += velocity * stepTime;
            rotation *= torque * stepTime;
        }
    }
}

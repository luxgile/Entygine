using Entygine.Mathematics;
using OpenToolkit.Mathematics;

namespace Entygine.Physics
{
    public class PhysicBody
    {
        public Vec3f position;
        public Quatf rotation;

        public Vec3f velocity;
        public Quatf torque;

        private Vec3f acumulatedForce;

        public void AddForce(Vec3f force)
        {
            acumulatedForce += force;
        }

        public void Step(float stepTime)
        {
            velocity += acumulatedForce;
            acumulatedForce = Vec3f.Zero;

            position += velocity * stepTime;
            rotation *= torque * stepTime;
        }
    }
}

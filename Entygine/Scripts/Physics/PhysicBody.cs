using Entygine.Mathematics;

namespace Entygine.Physics
{
    public class PhysicBody
    {
        public Vec3f position;
        public Quatf rotation;

        public bool isStatic;

        public Vec3f velocity;
        public Quatf torque;

        private Vec3f acumulatedForce;

        public void AddVelocity(Vec3f force)
        {
            acumulatedForce += force;
        }

        public void Step(float stepTime)
        {
            if (isStatic)
                return;

            velocity += acumulatedForce * stepTime;
            acumulatedForce = Vec3f.Zero;

            position += velocity * stepTime;
            rotation *= torque * stepTime;
        }
    }
}

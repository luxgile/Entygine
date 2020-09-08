using Entygine.Mathematics;
using System.Collections.Generic;

namespace Entygine.Physics
{
    public class PhysicsWorld
    {
        private List<PhysicBody> bodies = new List<PhysicBody>();

        public Vec3f Gravity { get; set; }

        public void RegisterBody(PhysicBody body)
        {
            bodies.Add(body);
        }

        public void StepPhysics(float dt)
        {
            for (int i = 0; i < bodies.Count; i++)
            {
                PhysicBody body = bodies[i];
                body.AddForce(Gravity);
                body.Step(dt);
            }
        }
    }
}

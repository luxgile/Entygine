using System.Collections.Generic;

namespace Entygine.Physics
{
    public class PhysicsCore
    {
        private List<PhysicBody> bodies = new List<PhysicBody>();

        public float StepTime { get; set; } = 0.02f;

        public void RegisterBody(PhysicBody body)
        {
            bodies.Add(body);
        }

        public void StepPhysics()
        {
            for (int i = 0; i < bodies.Count; i++)
                bodies[i].Step(StepTime);
        }
    }
}

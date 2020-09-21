using Entygine.Mathematics;
using System;
using System.Collections.Generic;

namespace Entygine.Physics
{
    public class PhysicsWorld
    {
        private Dictionary<int, PhysicBody> bodies = new Dictionary<int, PhysicBody>();

        public static PhysicsWorld Default { get; private set; }

        public Vec3f Gravity { get; set; } = new Vec3f(0, -9.8f, 0);

        public static void SetDefaultWorld(PhysicsWorld world)
        {
            Default = world;
        }

        public void UpdatePhysicsBody(int id, PhysicBody body)
        {
            bodies[id] = body;
        }

        public PhysicBody GetPhysicsBody(int id)
        {
            return bodies[id];
        }

        public int CreatePhysicsBody(PhysicBody body)
        {
            int id = new Random().Next();
            bodies.Add(id, body);
            return id;
        }

        public void StepPhysics(float dt)
        {
            //Apply forces
            foreach (KeyValuePair<int, PhysicBody> pair in bodies)
            {
                pair.Value.ApplyForce(Gravity);
                pair.Value.Step(dt);
            }

            //Detect Collisions
            //Solve constrains
        }
    }
}

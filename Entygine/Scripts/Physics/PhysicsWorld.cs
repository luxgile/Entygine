using BepuPhysics;
using BepuPhysics.Collidables;
using BepuPhysics.CollisionDetection;
using BepuUtilities.Memory;
using Entygine.Mathematics;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Entygine.Physics
{
    public class PhysicsWorld
    {
        private Dictionary<int, PhysicBody> bodies = new Dictionary<int, PhysicBody>();

        public static PhysicsWorld Default { get; private set; }

        public Vec3f Gravity { get; set; } = new Vec3f(0, -9.8f, 0);

        private BufferPool bufferPool;
        private Simulation simulation;

        public PhysicsWorld()
        {
            bufferPool = new BufferPool();
            simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks(), new PoseIntegratorCallbacks((Vector3)Gravity), new PositionLastTimestepper());
        }

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
            simulation.Timestep(dt);
            ////Apply forces
            //foreach (KeyValuePair<int, PhysicBody> pair in bodies)
            //{
            //    pair.Value.ApplyForce(Gravity);
            //    pair.Value.Step(dt);
            //}

            ////Detect Collisions
            ////Solve constrains
        }

        private struct NarrowPhaseCallbacks : INarrowPhaseCallbacks
        {
            public void Initialize(Simulation simulation) { }
            public bool AllowContactGeneration(int workerIndex, CollidableReference a, CollidableReference b)
            {
                return a.Mobility == CollidableMobility.Dynamic || b.Mobility == CollidableMobility.Dynamic;
            }

            public bool AllowContactGeneration(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB)
            {
                return true;
            }

            public bool ConfigureContactManifold<TManifold>(int workerIndex, CollidablePair pair, ref TManifold manifold, out PairMaterialProperties pairMaterial) where TManifold : struct, IContactManifold<TManifold>
            {
                pairMaterial.FrictionCoefficient = 1f;
                pairMaterial.MaximumRecoveryVelocity = 2f;
                pairMaterial.SpringSettings = new BepuPhysics.Constraints.SpringSettings(30, 1);
                return true;
            }

            public bool ConfigureContactManifold(int workerIndex, CollidablePair pair, int childIndexA, int childIndexB, ref ConvexContactManifold manifold)
            {
                return true;
            }

            public void Dispose() { }
        }

        public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
        {
            public Vector3 Gravity;
            Vector3 gravityDt;

            public AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving; //Don't care about fidelity in this demo!

            public PoseIntegratorCallbacks(Vector3 gravity) : this()
            {
                Gravity = gravity;
            }

            public void PrepareForIntegration(float dt)
            {
                gravityDt = Gravity * dt;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void IntegrateVelocity(int bodyIndex, in RigidPose pose, in BodyInertia localInertia, int workerIndex, ref BodyVelocity velocity)
            {
                if (localInertia.InverseMass > 0)
                {
                    velocity.Linear = velocity.Linear + gravityDt;
                }
            }

        }
    }
}

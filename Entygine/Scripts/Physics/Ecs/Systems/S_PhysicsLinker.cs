using Entygine.Ecs;
using Entygine.Mathematics;

namespace Entygine.Physics.Ecs
{
    [AfterSystem(typeof(S_PhysicsUpdateData))]
    public class S_PhysicsLinker : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.With(TypeCache.WriteType(typeof(C_PhysicsBody))).Any(TypeCache.ReadType(typeof(C_Position)));
            IterateQuery(new Iterator(), query);
        }

        private struct Iterator : IQueryEntityIterator
        {
            public void Iteration(ref EntityChunk chunk, int index)
            {
                if(chunk.TryGetComponent(index, out C_PhysicsBody physicsBody))
                {
                    if (physicsBody.id == 0)
                    {
                        physicsBody.id = PhysicsWorld.Default.CreatePhysicsBody(physicsBody.body);
                        chunk.SetComponent(index, physicsBody);
                    }
                    else
                        PhysicsWorld.Default.UpdatePhysicsBody(physicsBody.id, physicsBody.body);
                }
            }
        }
    }
}

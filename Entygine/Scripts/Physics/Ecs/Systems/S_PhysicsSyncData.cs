using Entygine.Cycles;
using Entygine.Ecs;
using OpenToolkit.Mathematics;

namespace Entygine.Physics.Ecs
{
    [AfterSystem(typeof(S_PhysicsLinker))]
    public class S_PhysicsSyncData : BaseSystem
    {
        private EntityQuery query = new EntityQuery();

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            query.With(TypeCache.ReadType(typeof(C_PhysicsBody)), TypeCache.WriteType(typeof(C_Position)));
            IterateQuery(new Iterator(), query);
        }

        private struct Iterator : IQueryEntityIterator
        {
            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (chunk.TryGetComponent(index, out C_PhysicsBody pb) && pb.id != 0 
                    && chunk.TryGetComponent(index, out C_Position position))
                {
                    PhysicBody body = PhysicsWorld.Default.GetPhysicsBody(pb.id);
                    position.value = (Vector3)body.position;
                    chunk.SetComponent(index, position);
                }
            }
        }
    }
}

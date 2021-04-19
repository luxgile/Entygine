using Entygine.Mathematics;

namespace Entygine.Ecs.Systems
{
    public struct C_BoxTag : IComponent { }

    public class S_BoxHover : BaseSystem
    {
        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            IterateQuery(new BoxIterator(), new EntityQuery().With(TypeCache.ReadType<C_BoxTag>(), TypeCache.WriteType<C_Position>()));
        }

        private struct BoxIterator : IQueryEntityIterator
        {
            public void Iteration(ref EntityChunk chunk, int index)
            {
                if (chunk.TryGetComponent(index, out C_Position position))
                {
                    position.value.y += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
                    chunk.SetComponent(index, position);
                }
            }
        }
    }
}

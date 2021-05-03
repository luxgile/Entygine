using Entygine.Mathematics;

namespace Entygine.Ecs.Systems
{
    public struct C_BoxTag : IComponent { }

    public class S_BoxHover : BaseSystem
    {
        private EntityQuerySettings query = new EntityQuerySettings().With(TypeCache.ReadType<C_BoxTag>(), TypeCache.WriteType<C_Position>());

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            IterateQuery(new BoxIterator(), query);

            //TODO: This is what iterations should look like, to avoid complicated stuff. Have in mind it should allow chunk iteration and entity iteration
            SetupQuery(query).Iteration((context) =>
            {
                context.Read(out C_Position position);
                position.value += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
                context.Write(position);
            }).Perform();
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

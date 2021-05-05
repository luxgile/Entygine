using Entygine.Mathematics;

namespace Entygine.Ecs.Systems
{
    public struct C_BoxTag : IComponent { }

    public class BoxHoverSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(TypeCache.ReadType<C_BoxTag>(), TypeCache.WriteType<C_Position>());

        protected override QueryScope SetupQuery()
        {
            return new EntityQueryScope(settings, (context) =>
            {
                context.Read(out C_Position position);
                position.value.y += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
                context.Write(position);
            });
        }
    }
}

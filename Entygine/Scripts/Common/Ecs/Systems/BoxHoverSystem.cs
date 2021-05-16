using Entygine.Mathematics;

namespace Entygine.Ecs.Systems
{
    public partial struct C_BoxTag : IComponent { }

    public class BoxHoverSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(C_BoxTag.Identifier, C_Position.Identifier);

        protected override QueryScope SetupQuery()
        {
            return null;
            //return new EntityQueryScope(settings, (ref EntityQueryContext context) =>
            //{
            //    context.Read(C_Position.Identifier, out C_Position position);
            //    position.value.y += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
            //    context.Write(C_Position.Identifier, position);
            //});
        }

        protected override void OnPerformFrame(float dt)
        {
            base.OnPerformFrame(dt);

            //new EntityQueryScope(settings, (ref EntityQueryContext x) => { }).Iterate((ref C_Position position) =>
            //{
            //    position.value.y += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
            //});
        }
    }
}

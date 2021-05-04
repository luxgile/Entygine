using Entygine.Mathematics;

namespace Entygine.Ecs.Systems
{
    public struct C_BoxTag : IComponent { }

    public class S_BoxHover : BaseSystem
    {
        private readonly EntityQuerySettings query = new EntityQuerySettings().With(TypeCache.ReadType<C_BoxTag>(), TypeCache.WriteType<C_Position>());
        private EntityQueryScope scope;

        protected override void OnSystemCreated()
        {
            scope = (EntityQueryScope)new EntityQueryScope().Setup(query).Iteration((context) =>
            {
                context.Read(out C_Position position);
                position.value.y += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
                context.Write(position);
            });
        }

        protected override void OnPerformFrame(float dt)
        {
            //TODO: Make new System Iterator where most of this is handled in the back
            scope.OnlyChanged(LastVersionWorked).Perform();
            asdasdasd
            //TODO: This is what iterations should look like, to avoid complicated stuff. Have in mind it should allow chunk iteration and entity iteration
            //SetupQuery(query).Iteration((context) =>
            //{
            //    context.Read(out C_Position position);
            //    position.value += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
            //    context.Write(position);
            //}).Perform();
        }
    }
}

using Entygine.Cycles;
using OpenTK.Mathematics;

namespace Entygine.Ecs
{
    [SystemGroup(typeof(MainPhases.LatePhaseId))]
    public class TransformConverterSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings().With(TypeCache.WriteType(typeof(C_Transform)))
                .Any(TypeCache.ReadType(typeof(C_Position)), TypeCache.ReadType(typeof(C_Rotation)), TypeCache.ReadType(typeof(C_UniformScale)));

        protected override QueryScope SetupQuery()
        {
            return new EntityQueryScope(settings, (context) =>
            {
                context.Read(out C_Position positionComponent);
                Vector3 pos = (Vector3)positionComponent.value;

                context.Read(out C_Rotation rotationComponent);
                Quaternion rot = rotationComponent.value;

                context.Read(out C_UniformScale scaleComponent);
                Vector3 scale = Vector3.One * scaleComponent.value;

                Matrix4 transform = Matrix4.Identity;
                transform *= Matrix4.CreateScale(scale);
                transform *= Matrix4.CreateFromQuaternion(rot);
                transform *= Matrix4.CreateTranslation(pos);
                context.Write(new C_Transform() { value = transform });
            });
        }
    }
}
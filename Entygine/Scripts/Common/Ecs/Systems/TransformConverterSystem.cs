using Entygine.Cycles;
using OpenTK.Mathematics;

namespace Entygine.Ecs
{
    [SystemGroup(typeof(MainPhases.LatePhaseId))]
    public class TransformConverterSystem : QuerySystem
    {
        private readonly QuerySettings settings = new QuerySettings()
            .With(C_Transform.Identifier)
            .Any(C_Position.Identifier, C_Rotation.Identifier, C_UniformScale.Identifier);

        protected override QueryScope SetupQuery()
        {
            return new EntityQueryScope(settings, (ref EntityQueryContext context) =>
            {
                bool willWrite = false;

                context.Read(C_Transform.Identifier, out C_Transform transformComponent);
                Matrix4 prevTransform = transformComponent.value;
                Matrix4 transform = Matrix4.Identity;
                if (prevTransform.M44 == 0)
                {
                    prevTransform = transform;
                    willWrite = true;
                }

                Vector3 scale = Vector3.One;
                if (context.Read(C_UniformScale.Identifier, out C_UniformScale scaleComponent))
                {
                    scale *= scaleComponent.value;
                    willWrite = true;
                }

                Quaternion rot = Quaternion.Identity;
                if (context.Read(C_Rotation.Identifier, out C_Rotation rotationComponent))
                {
                    rot = rotationComponent.value;
                    willWrite = true;
                }

                Vector3 pos = Vector3.Zero;
                if (context.Read(C_Position.Identifier, out C_Position positionComponent) && (Vector3)positionComponent.value != prevTransform.ExtractTranslation())
                {
                    pos = (Vector3)positionComponent.value;
                    willWrite = true;
                }

                transform *= Matrix4.CreateScale(scale);
                transform *= Matrix4.CreateFromQuaternion(rot);
                transform *= Matrix4.CreateTranslation(pos);

                if (willWrite)
                    context.Write(C_Transform.Identifier, new C_Transform() { value = transform });
            });
        }
    }
}
﻿using Entygine.Cycles;
using Entygine.Ecs;
using OpenToolkit.Mathematics;

namespace Entygine.Rendering
{
    [SystemGroup(typeof(DefaultPhaseId))]
    public class S_TransformConverter : BaseSystem
    {
        private EntityQuery query;

        protected override void OnSystemCreated()
        {
            base.OnSystemCreated();

            query = new EntityQuery(World).With(TypeCache.WriteType(typeof(C_Transform)))
                .Any(TypeCache.ReadType(typeof(C_Position)), TypeCache.ReadType(typeof(C_Rotation)), TypeCache.ReadType(typeof(C_UniformScale)));
        }

        protected override void OnPerformFrame()
        {
            base.OnPerformFrame();

            query.Perform(new Iterator(), LastVersionWorked);
        }

        private struct Iterator : IQueryEntityIterator
        {
            public void Iteration(ref EntityChunk chunk, int index)
            {
                Vector3 pos = Vector3.Zero;
                if (chunk.TryGetComponent(index, out C_Position positionComponent))
                    pos = positionComponent.value;

                Quaternion rot = Quaternion.Identity;
                if (chunk.TryGetComponent(index, out C_Rotation rotationComponent))
                    rot = rotationComponent.value;

                Vector3 scale = Vector3.One;
                if (chunk.TryGetComponent(index, out C_UniformScale scaleComponent))
                    scale *= scaleComponent.value;

                Matrix4 transform = Matrix4.Identity;
                transform *= Matrix4.CreateScale(scale);
                transform *= Matrix4.CreateFromQuaternion(rot);
                transform *= Matrix4.CreateTranslation(pos);
                chunk.SetComponent(index, new C_Transform() { value = transform });
            }
        }
    }
}

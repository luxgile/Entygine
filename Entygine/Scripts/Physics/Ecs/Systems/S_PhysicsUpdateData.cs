﻿using Entygine.Cycles;
using Entygine.Ecs;
using Entygine.Mathematics;
using OpenToolkit.Mathematics;

namespace Entygine.Physics.Ecs
{
    [BeforeSystem(typeof(S_PhysicsLinker))]
    public class S_PhysicsUpdateData : BaseSystem
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
                if (chunk.TryGetComponent(index, out C_PhysicsBody physicsBody))
                {
                    if (chunk.TryGetComponent(index, out C_Position pos))
                        physicsBody.body.position = (Vec3f)pos.value;

                    chunk.SetComponent(index, physicsBody);
                }
            }
        }
    }
}
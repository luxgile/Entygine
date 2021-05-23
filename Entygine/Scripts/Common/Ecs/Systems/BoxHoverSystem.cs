using Entygine.Mathematics;
using Entygine.Ecs;
using System.Collections.Generic;

namespace Entygine.Ecs.Systems
{
    public partial struct C_BoxTag : IComponent { }

    public class BoxHoverSystem : QuerySystem<EntityIterator>
    {
        protected override void OnFrame(float dt)
        {
            Iterator.With(C_BoxTag.Identifier).Iterate((ref C_Position position) =>
            {
                position.value.y += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
            });
        }
    }
}

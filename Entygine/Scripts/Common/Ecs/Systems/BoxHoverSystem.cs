using Entygine.Mathematics;
using Entygine.Ecs;
using System.Collections.Generic;

namespace Entygine.Ecs.Systems
{
    public partial struct C_BoxTag : IComponent { }

    internal class BoxHoverSystem : QuerySystem
    {
        protected override void OnFrame(float dt)
        {
            Iterator.SetName("Box Hovering").RWith(C_BoxTag.Identifier).Iterate((ref C_Position position) =>
            {
                position.value.y += (MathUtils.Sin((float)EntygineApp.EngineTime)) * 0.01f;
            });
        }
    }
}

using Entygine.Ecs;
using Entygine.Ecs.Components;
using ImGuiNET;
using System;
using System.Numerics;

namespace Entygine_Editor
{
    public class ClientWindow : WindowDrawer
    {
        public override string Title => "Client";

        protected override void OnDraw()
        {
            EntityIterator.PerformIteration(EntityWorld.Active, new RenderCamera(), new EntityQuery().Any(TypeCache.ReadType<C_Camera>()));
        }

        private struct RenderCamera : IQueryEntityIterator
        {
            public void Iteration(ref EntityChunk chunk, int index)
            {
                chunk.TryGetComponent(index, out C_Camera camera);
                ImGui.Image((IntPtr)camera.cameraData.ColorTargetTexture.handle, new Vector2(800, 600), new Vector2(0, 0), new Vector2(1, -1));
            }
        }
    }
}

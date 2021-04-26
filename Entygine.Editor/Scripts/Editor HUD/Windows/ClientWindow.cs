using Entygine;
using Entygine.Ecs;
using Entygine.Ecs.Components;
using Entygine.Mathematics;
using ImGuiNET;
using System;
using System.Numerics;

namespace Entygine_Editor
{
    public class ClientWindow : WindowDrawer
    {
        public override string Title => "Client";

        private bool keepAspect = true;

        private Vector2 prevSize;
        private Vector2 currSize;

        public ClientWindow()
        {
            Flags |= ImGuiWindowFlags.MenuBar;
        }

        protected override void OnPreDraw()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        }

        protected override void OnPostDraw()
        {
            ImGui.PopStyleVar();
        }

        protected override void OnDraw()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("Settings"))
                {
                    if (ImGui.MenuItem("Keep Aspect", "", keepAspect, true))
                        keepAspect = !keepAspect;

                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }

            currSize = ImGui.GetContentRegionAvail();
            if (keepAspect)
                currSize = GetAspectArea(currSize);

            if (prevSize != currSize)
                AppScreen.Resolution = (Vec2i)currSize;
            prevSize = currSize;

            EntityIterator.PerformIteration(EntityWorld.Active, new RenderCamera() { imageSize = currSize }, new EntityQuery().Any(TypeCache.ReadType<C_Camera>()));
        }

        private Vector2 GetAspectArea(Vector2 avail)
        {
            float aspect = 16f / 9f;
            Vector2 area = avail;
            area.X = area.Y * aspect;
            if (area.X > avail.X)
            {
                area.X = avail.X;
                area.Y = area.X * (1 / aspect);
            }
            return area;
        }

        private struct RenderCamera : IQueryEntityIterator
        {
            public Vector2 imageSize;
            public void Iteration(ref EntityChunk chunk, int index)
            {
                chunk.TryGetComponent(index, out C_Camera camera);
                Vector2 floatingSize = (ImGui.GetContentRegionAvail() - imageSize) / 2f;
                ImGui.SetCursorPos(ImGui.GetCursorPos() + floatingSize);
                ImGui.Image((IntPtr)camera.cameraData.FinalFramebuffer.ColorBuffer, imageSize, new Vector2(0, 0), new Vector2(1, -1));
            }
        }
    }
}

using Entygine.Mathematics;
using ImGuiNET;
using System.Numerics;

namespace Entygine_Editor
{
    public abstract class WindowDrawer : RawDrawer
    {
        public ImGuiWindowFlags Flags { get; set; }

        public sealed override bool Draw()
        {
            OnPreDraw();
            bool open = ImGui.Begin(Title, Flags);
            if (open)
                OnDraw();
            ImGui.End();
            OnPostDraw();
            return open;
        }
        protected virtual void OnPreDraw() { }
        protected virtual void OnDraw() { }
        protected virtual void OnPostDraw() { }

        public abstract string Title { get; }
    }
}

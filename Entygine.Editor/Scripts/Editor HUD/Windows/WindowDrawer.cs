using ImGuiNET;
using static ImGuiNET.ImGui;

namespace Entygine_Editor
{
    public abstract class WindowDrawer : RawDrawer
    {
        public ImGuiWindowFlags Flags { get; set; }

        public override bool Draw()
        {
            bool open = Begin(Title, Flags);
            if (open)
                OnDraw();
            End();
            return open;
        }
        protected virtual void OnDraw() { }

        public abstract string Title { get; }
    }
}

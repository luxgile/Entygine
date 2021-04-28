using Entygine.Ecs;
using ImGuiNET;

namespace Entygine_Editor
{
    public class EntityDetailsDrawer : DetailsDrawer<Entity>
    {
        public override int MatchesObject(object obj)
        {
            return obj is Entity ? 0 : -1;
        }

        protected override void OnDraw()
        {
            ImGui.Text($"ID: {Context.id}");
            ImGui.Text($"Version: {Context.version}");
        }
    }
}

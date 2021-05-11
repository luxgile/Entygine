using Entygine.Ecs;
using ImGuiNET;
using System;
using System.Runtime.InteropServices;

namespace Entygine_Editor
{
    public class ChunkDetailsDrawer : DetailsDrawer<EntityChunk>
    {
        public override int MatchesObject(object obj)
        {
            return obj is EntityChunk ? 0 : -1;
        }

        protected override void OnDraw()
        {
            base.OnDraw();

            EntityChunk chunk = Context;
            ImGui.Text($"Entity count: {chunk.Count}/{chunk.Capacity}");
            ImGui.Text($"Size in bytes: ???");
            ImGui.Separator();
            ImGui.Text("Archetype:");

            EntityArchetype archetype = chunk.Archetype;
            TypeCache[] sharedTypes = archetype.GetSharedTypes();
            TypeCache[] componentTypes = archetype.GetComponenTypes();

            if(sharedTypes.Length == 0)
            {
                ImGui.TreeNodeEx("", ImGuiTreeNodeFlags.Bullet);
                ImGui.SameLine();
                ImGui.TextDisabled("Shared");
            }
            else if (ImGui.TreeNodeEx("Shared", ImGuiTreeNodeFlags.DefaultOpen))
            {
                for (int i = 0; i < sharedTypes.Length; i++)
                    ImGui.BulletText(sharedTypes[i].Type.Name);
                ImGui.TreePop();
            }

            if(componentTypes.Length == 0)
            {
                ImGui.TreeNodeEx("", ImGuiTreeNodeFlags.Bullet);
                ImGui.SameLine();
                ImGui.TextDisabled("Instanced");
            }
            else if (ImGui.TreeNodeEx("Instanced", ImGuiTreeNodeFlags.DefaultOpen))
            {
                for (int i = 0; i < componentTypes.Length; i++)
                    ImGui.BulletText(componentTypes[i].Type.Name);
                ImGui.TreePop();
            }
        }
    }
}

using Entygine;
using Entygine.Ecs;
using ImGuiNET;

namespace Entygine_Editor
{
    public class WorldWindow : WindowDrawer
    {
        public override string Title => "World";

        protected override void OnDraw()
        {
            EntityWorld world = EntityWorld.Active;
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
                //ImGui.Text(chunk.Archetype.ToString());
                //ImGui.SameLine();
                bool open = ImGui.TreeNode($"[{i}] Chunk: {chunk.Count}/{chunk.Capacity}");
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip(chunk.Archetype.ToString());
                if (open)
                {
                    ImGuiTableFlags tFlags = ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable;
                    if (ImGui.BeginTable("Entities", 2, tFlags))
                    {
                        ImGui.TableSetupColumn("Id");
                        ImGui.TableSetupColumn("Version");
                        ImGui.TableHeadersRow();

                        for (int e = 0; e < chunk.Count; e++)
                        {
                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();
                            ImGui.Text(chunk.GetEntity(e).id.ToString());

                            ImGui.TableNextColumn();
                            ImGui.Text(chunk.GetEntity(e).version.ToString());
                        }
                        ImGui.Separator();
                        ImGui.EndTable();
                    }
                    ImGui.TreePop();
                }
            }
        }
    }
}

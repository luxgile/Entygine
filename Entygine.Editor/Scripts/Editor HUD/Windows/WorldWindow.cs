using Entygine;
using Entygine.Ecs;
using ImGuiNET;
using System;

namespace Entygine_Editor
{
    public class WorldWindow : WindowDrawer
    {
        public override string Title => "World";
        private int selectedView;
        private string[] views = new string[] { "Raw", "Chunks", "Hierarchy" };

        protected override void OnDraw()
        {
            ImGui.Combo("Mode", ref selectedView, views, views.Length);
            ImGui.Separator();
            switch (selectedView)
            {
                case 0:
                DrawRaw();
                break;

                case 1:
                DrawChunks();
                break;

                case 2:
                DrawHierarchy();
                break;
            }
        }

        private void DrawHierarchy()
        {
            ImGui.Text("Not yet supported...");
        }

        private void DrawChunks()
        {
            EntityWorld world = EntityWorld.Active;
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            for (int i = 0; i < chunks.Count; i++)
            {
                ref EntityChunk chunk = ref chunks[i];
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
                            Entity entity = chunk.GetEntity(e);

                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();

                            bool isSelected = ObjectSelections.CurrentObj?.Equals(entity) ?? false;
                            if (ImGui.Selectable($"##{e}", isSelected, ImGuiSelectableFlags.SpanAllColumns))
                            {
                                ObjectSelections.SelectObject(entity);
                            }
                            ImGui.SameLine();
                            ImGui.Text(entity.id.ToString());

                            ImGui.TableNextColumn();
                            ImGui.Text(entity.version.ToString());
                        }
                        ImGui.Separator();
                        ImGui.EndTable();
                    }
                    ImGui.TreePop();
                }
            }
        }

        private void DrawRaw()
        {
            EntityWorld world = EntityWorld.Active;
            StructArray<EntityChunk> chunks = world.EntityManager.GetChunks();
            ImGuiTableFlags tFlags = ImGuiTableFlags.RowBg | ImGuiTableFlags.Resizable;
            if (ImGui.BeginTable("Entities", 2, tFlags))
            {
                ImGui.TableSetupColumn("Id");
                ImGui.TableSetupColumn("Version");
                ImGui.TableHeadersRow();
                for (int i = 0; i < chunks.Count; i++)
                {
                    ref EntityChunk chunk = ref chunks[i];

                    for (int e = 0; e < chunk.Count; e++)
                    {
                        Entity entity = chunk.GetEntity(e);

                        ImGui.TableNextRow();
                        ImGui.TableNextColumn();

                        bool isSelected = ObjectSelections.CurrentObj?.Equals(entity) ?? false;
                        if (ImGui.Selectable($"##{e}", isSelected, ImGuiSelectableFlags.SpanAllColumns))
                        {
                            ObjectSelections.SelectObject(entity);
                        }
                        ImGui.SameLine();
                        ImGui.Text(entity.id.ToString());

                        ImGui.TableNextColumn();
                        ImGui.Text(entity.version.ToString());
                    }
                }

                ImGui.EndTable();
            }
        }
    }
}

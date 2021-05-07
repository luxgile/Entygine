using Entygine;
using Entygine.Cycles;
using Entygine.Ecs;
using ImGuiNET;

namespace Entygine_Editor
{
    public class WorldWindow : WindowDrawer
    {
        public override string Title => "World";
        private int selectedView = 1;
        private int tab = 0;
        private string[] views = new string[] { "Raw", "Chunks", "Hierarchy" };

        protected override void OnDraw()
        {
            ImGui.BeginTabBar("Tabs");

            if (ImGui.TabItemButton("Entities"))
                tab = 0;

            if (ImGui.TabItemButton("System"))
                tab = 1;

            ImGui.EndTabBar();

            if (tab == 0)
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
            else
            {
                DrawSystems();
            }
        }

        private void DrawSystems()
        {
            var systems = EntityWorld.Active.Runner.GetSystems<MainPhases.DefaultPhaseId>();
            for (int i = 0; i < systems.Length; i++)
            {
                BaseSystem currSystem = systems[i];
                ImGui.Text(currSystem.GetType().Name);
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
            ImGuiTableFlags tFlags = ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit;
            if (ImGui.BeginTable("Entities", 3, tFlags))
            {
                ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("Id");
                ImGui.TableSetupColumn("Version");
                ImGui.TableHeadersRow();

                ImGui.PushID("Chunks_");
                for (int i = 0; i < chunks.Count; i++)
                {
                    ref EntityChunk chunk = ref chunks[i];
                    bool isSelected = ObjectSelections.CurrentObj?.Equals(chunk) ?? false;

                    ImGui.TableNextRow();
                    ImGui.TableNextColumn();

                    ImGuiTreeNodeFlags flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.SpanFullWidth;
                    if (isSelected)
                        flags |= ImGuiTreeNodeFlags.Selected;

                    bool open = ImGui.TreeNodeEx($"Chunk: {chunk.Count}/{chunk.Capacity}", flags);
                    if (ImGui.IsItemClicked())
                        ObjectSelections.SelectObject(chunk);

                    ImGui.TableNextColumn();
                    ImGui.Text("-");

                    ImGui.TableNextColumn();
                    ImGui.Text(chunk.ChunkVersion.ToString());


                    //if (ImGui.IsItemHovered())
                    //    ImGui.SetTooltip(chunk.Archetype.ToString());

                    if (open)
                    {
                        ImGui.PushID("Entities_");
                        for (int e = 0; e < chunk.Count; e++)
                        {
                            Entity entity = chunk.GetEntity(e);

                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();
                            

                            isSelected = ObjectSelections.CurrentObj?.Equals(entity) ?? false;
                            flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.SpanFullWidth | ImGuiTreeNodeFlags.Bullet;
                            if (isSelected)
                                flags |= ImGuiTreeNodeFlags.Selected;

                            ImGui.TreeNodeEx("Entity", flags);
                            if (ImGui.IsItemClicked())
                                ObjectSelections.SelectObject(entity);

                            ImGui.TableNextColumn();
                            ImGui.Text(entity.id.ToString());

                            ImGui.TableNextColumn();
                            ImGui.Text(entity.version.ToString());
                        }
                        ImGui.PopID();
                        ImGui.TreePop();
                    }
                }
                ImGui.PopID();
                ImGui.EndTable();
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

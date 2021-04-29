using Entygine.DevTools;
using ImGuiNET;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;

namespace Entygine_Editor
{
    public class ConsoleWindow : WindowDrawer, IConsoleLogger
    {
        private int indexSelected;
        private List<LogData> logs = new List<LogData>();

        private ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit;

        public ConsoleWindow()
        {
            DevConsole.AddLogger(this);
        }
        protected override void OnPreDraw()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        }

        protected override void OnPostDraw()
        {
            ImGui.PopStyleVar();
        }

        public void Log(LogData log)
        {
            logs.Add(log);
        }

        public void Clear()
        {
            logs.Clear();
        }

        protected override void OnDraw()
        {
            ImGui.BeginTable("logs", 3, tableFlags);
            ImGui.TableSetupColumn("Date", ImGuiTableColumnFlags.None, 150);
            ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.None, 70);
            ImGui.TableSetupColumn("Log", ImGuiTableColumnFlags.None);
            ImGui.TableHeadersRow();
            for (int i = 0; i < logs.Count; i++)
            {
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                bool isSelected = indexSelected == i;
                if (ImGui.Selectable("##" + i, isSelected, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(0))
                    indexSelected = i;

                if (isSelected)
                {
                    ImGui.PushItemWidth(ImGui.GetWindowSize().X);
                    ImGui.Text(logs[i].trace.ToString());
                    ImGui.Separator();
                    ImGui.PopItemWidth();
                }

                ImGui.SameLine();
                ImGui.Text(logs[i].Date.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(logs[i].type.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(logs[i].log.ToString());
            }
            ImGui.EndTable();
        }
        public override string Title => "Console";
    }
}

using Entygine.DevTools;
using Entygine.Rendering;
using ImGuiNET;
using System.Collections.Generic;
using System.Numerics;

namespace Entygine_Editor
{
    public class ConsoleWindow : WindowDrawer, IConsoleLogger
    {
        private int indexSelected = -1;
        private List<LogData> logs = new List<LogData>();

        private ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit;

        public ConsoleWindow()
        {
            DevConsole.AddLogger(this);
            Flags |= ImGuiWindowFlags.HorizontalScrollbar;
        }
        //protected override void OnPreDraw()
        //{
        //    ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, Vector2.Zero);
        //}

        //protected override void OnPostDraw()
        //{
        //    ImGui.PopStyleVar();
        //}

        public void Log(LogData log)
        {
            logs.Add(log);
        }

        public void Clear()
        {
            logs.Clear();
        }

        private bool GetColorForLog(LogData log, out Color01 color)
        {
            switch (log.type)
            {
                default:
                case LogType.VeryVerbose:
                case LogType.Verbose:
                case LogType.Info:
                color = Color01.white;
                return false;

                case LogType.Warning:
                color = new Color01(0.3f, 0.3f, 0f, 1f);
                return true;

                case LogType.Error:
                color = new Color01(0.3f, 0f, 0f, 1f);
                return true;
            }
        }

        protected override void OnDraw()
        {
            ImGui.BeginTable("logs", 3, tableFlags);
            ImGui.TableSetupColumn("Date", ImGuiTableColumnFlags.None, 150);
            ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.None, 70);
            ImGui.TableSetupColumn("Log", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableHeadersRow();
            for (int i = 0; i < logs.Count; i++)
            {
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                bool isSelected = indexSelected == i;
                if (ImGui.Selectable("##" + i, isSelected, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(0))
                    indexSelected = isSelected ? -1 : i;

                ImGui.SameLine();
                ImGui.Text(logs[i].Date.ToString());

                ImGui.TableNextColumn();

                if (GetColorForLog(logs[i], out Color01 color))
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, ImGui.ColorConvertFloat4ToU32((Vector4)color));

                ImGui.Text(logs[i].type.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(logs[i].log.ToString());

                if (isSelected)
                {
                    ImGui.BeginChild("stacktrace");
                    ImGui.Text(logs[i].trace.ToString());
                    ImGui.Separator();
                    ImGui.EndChild();
                }
            }
            ImGui.EndTable();
        }
        public override string Title => "Console";
    }
}

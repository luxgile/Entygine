using Entygine.DevTools;
using Entygine.Rendering;
using Entygine_Editor.IDE;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Entygine_Editor
{
    public class ConsoleWindow : WindowDrawer, IConsoleLogger
    {
        private int currentTab = -1;
        private List<int> indexSelected = new List<int>();
        private List<LogData> logs = new List<LogData>();

        private ImGuiTableFlags tableFlags = ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit;

        public ConsoleWindow()
        {
            DevConsole.AddLogger(this);
            Flags |= ImGuiWindowFlags.HorizontalScrollbar | ImGuiWindowFlags.MenuBar;
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
                color = new Color01(0.3f, 0.3f, 0.3f, 0.7f);
                return true;

                case LogType.Warning:
                color = new Color01(0.3f, 0.3f, 0f, 0.7f);
                return true;

                case LogType.Error:
                color = new Color01(0.3f, 0f, 0f, 0.7f);
                return true;
            }
        }

        protected override void OnDraw()
        {
            if (ImGui.BeginMenuBar())
            {
                if (ImGui.BeginMenu("View"))
                {
                    if (ImGui.MenuItem("Clear"))
                        Clear();

                    ImGui.EndMenu();
                }
                ImGui.EndMenuBar();
            }

            if (indexSelected.Count > 0)
                currentTab = DrawTabs();

            if (currentTab == -1)
                DrawTable();
            else
                DrawSelected(currentTab);
        }

        private int DrawTabs()
        {
            ImGui.BeginTabBar("tabs");

            if (ImGui.TabItemButton("Console"))
                return -1;

            for (int i = 0; i < indexSelected.Count; i++)
            {
                int index = indexSelected[i];
                if (ImGui.TabItemButton($"{logs[i].type}: {index}", index == currentTab ? ImGuiTabItemFlags.SetSelected : ImGuiTabItemFlags.None))
                    return index;
            }
            ImGui.EndTabBar();
            return currentTab;
        }

        private void DrawSelected(int selected)
        {
            LogData log = logs[selected];
            ImGui.Text(log.log.ToString());
            ImGui.Separator();
            ImGui.BeginTable("stacktrace", 3, ImGuiTableFlags.RowBg | ImGuiTableFlags.Borders | ImGuiTableFlags.SizingStretchProp);
            ImGui.TableSetupColumn("Depth");
            ImGui.TableSetupColumn("Path");
            ImGui.TableSetupColumn("Method");
            ImGui.TableHeadersRow();

            string stack = log.trace.ToString();
            string[] lines = stack.Split('\n');
            for (int i = 0; i < lines.Length - 1; i++)
            {
                string line = lines[i].Trim();
                int atIndex = line.IndexOf("at ");
                int inIndex = line.IndexOf(" in ");
                string method = line;
                if (inIndex != -1)
                {
                    method = method[atIndex..inIndex].Remove(0, 3);
                }

                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                if (ImGui.Selectable("##" + i, false, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(0))
                {
                    if (inIndex != -1)
                    {
                        string path = line[inIndex..^0];
                        path = path.Remove(0, 3);
                        string a = path[0..path.IndexOf(".cs:")] + ".cs";
                        string lineCount = path[(path.IndexOf("line ") + 5)..^0];
                        VisualStudioUtils.OpenVS(a, int.Parse(lineCount));
                    }
                }

                ImGui.SameLine();
                ImGui.Text(i.ToString());

                ImGui.TableNextColumn();
                if (line.Contains('\\'))
                    ImGui.Text(line.Split('\\')[^1]);
                else
                {
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, ImGui.ColorConvertFloat4ToU32((Vector4)new Color01(0.3f, 0.3f, 0f, 0.7f)));
                    ImGui.Text("External code");
                }

                ImGui.TableNextColumn();
                ImGui.TextWrapped(method);
            }

            ImGui.EndTable();
        }

        private void DrawTable()
        {
            ImGui.BeginTable("logs", 3, tableFlags);
            ImGui.TableSetupColumn("Date", ImGuiTableColumnFlags.None, 100);
            ImGui.TableSetupColumn("Type", ImGuiTableColumnFlags.None, 70);
            ImGui.TableSetupColumn("Log", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableHeadersRow();
            for (int i = logs.Count - 1; i >= 0; i--)
            {
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                if (ImGui.Selectable("##" + i, false, ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowDoubleClick) && ImGui.IsMouseDoubleClicked(0)
                    && !indexSelected.Contains(i))
                {
                    indexSelected.Add(i);
                    currentTab = i;
                }

                ImGui.SameLine();
                ImGui.Text(logs[i].Date.ToLongTimeString());

                ImGui.TableNextColumn();

                if (GetColorForLog(logs[i], out Color01 color))
                    ImGui.TableSetBgColor(ImGuiTableBgTarget.CellBg, ImGui.ColorConvertFloat4ToU32((Vector4)color));

                ImGui.Text(logs[i].type.ToString());

                ImGui.TableNextColumn();
                ImGui.Text(logs[i].log.ToString());
            }
            ImGui.EndTable();
        }

        public override string Title => "Console";
    }
}

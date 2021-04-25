using Entygine.DevTools;
using ImGuiNET;
using System.Collections.Generic;

namespace Entygine_Editor
{
    public class ConsoleWindow : WindowDrawer, IConsoleLogger
    {
        private List<string> logs = new List<string>();

        public ConsoleWindow()
        {
            DevConsole.AddLogger(this);
        }

        public void Log(LogData log)
        {
            logs.Add(log.ToString());
        }

        public void Clear()
        {
            logs.Clear();
        }

        protected override void OnDraw()
        {
            ImGui.Begin("Console", ImGuiWindowFlags.NoCollapse);
            for (int i = 0; i < logs.Count; i++)
            {
                ImGui.Text(logs[i]);
            }
        }
        public override string Title => "Console";
    }
}

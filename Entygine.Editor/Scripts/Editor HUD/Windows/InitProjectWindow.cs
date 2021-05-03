using ImGuiNET;
using System.Numerics;

namespace Entygine_Editor
{
    public class InitProjectWindow : RawDrawer
    {
        private bool creatingProject;
        private string projectPath = "";
        private string projectName = "";

        public override bool Draw()
        {
            var size = (MainEditorWindow.Window.Size.ToVector2() * 0.5f);
            ImGui.SetNextWindowPos(new Vector2(size.X, size.Y), ImGuiCond.Always, Vector2.One / 2f);
            ImGui.Begin("", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoBringToFrontOnFocus);
            if (ImGui.Button("Create"))
            {
                creatingProject = true;
            }
            ImGui.SameLine();
            if (ImGui.Button("Open") && Platform.OpenFolderBroswer(out string path))
            {
                EditorProject.OpenProject(path);
            }
            ImGui.SameLine();
            ImGui.BeginGroup();
            if (ImGui.Button("Open last"))
            {
                EditorProject.OpenProject(EngineEditorSettings.Current.ProjMeta.LastProjectOpened);
            }
            ImGui.Text("LAST PROJECT");
            ImGui.EndGroup();
            ImGui.End();

            if (creatingProject)
                ProjectSetupDraw();

            return true;
        }

        private void ProjectSetupDraw()
        {
            var size = (MainEditorWindow.Window.Size.ToVector2() * 0.5f);
            ImGui.SetNextWindowPos(new Vector2(size.X, size.Y), ImGuiCond.Always, Vector2.One / 2f);
            ImGui.Begin("Setuppp", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize 
                | ImGuiWindowFlags.AlwaysAutoResize);
            ImGui.InputText("Name", ref projectName, (uint)projectName.Length);
            ImGui.InputText("Path", ref projectPath, (uint)projectPath.Length, ImGuiInputTextFlags.ReadOnly);
            ImGui.SameLine();

            if (ImGui.Button("Browse"))
                Platform.OpenFolderBroswer(out projectPath);

            bool isInvalid = string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(projectPath);
            if (ImGui.Button("Close"))
            {
                creatingProject = false;
            }
            ImGui.SameLine();
            if (ImGui.Button("Create"))
            {
                if (!isInvalid)
                {
                    EditorProject.CreateProject(projectPath, projectName);
                    EditorProject.OpenProject(projectPath);
                }
            }
            ImGui.End();
        }
    }
}

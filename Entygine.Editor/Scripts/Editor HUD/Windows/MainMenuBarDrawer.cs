using ImGuiNET;

namespace Entygine_Editor
{
    public class MainMenuBarDrawer : RawDrawer
    {
        public override bool Draw()
        {
            ImGui.DockSpaceOverViewport();
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.BeginMenu("Project"))
                {
                    if (ImGui.MenuItem("New") && Platform.OpenFolderBroswer(out string path))
                        EditorProject.CreateProject(path, "Template Project");

                    if (ImGui.MenuItem("Open") && Platform.OpenFolderBroswer(out path))
                        EditorProject.OpenProject(path);

                    if (ImGui.MenuItem("Open in Explorer"))
                        Platform.OpenExplorerFolder(EditorProject.CurrentProjectPath);

                    ImGui.EndMenu();
                }

                ImGui.EndMenu();
            }
            //if (ImGui.BeginMenu("Windows"))
            //{
            //    if (ImGui.MenuItem("Style Editor"))
            //        showstyle = !showstyle;

            //    ImGui.EndMenu();
            //}
            ImGui.EndMainMenuBar();
            return true;
        }
    }
}

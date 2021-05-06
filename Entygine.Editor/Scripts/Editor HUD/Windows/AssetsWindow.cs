using ImGuiNET;
using System.IO;

namespace Entygine_Editor
{
    public class AssetsWindow : WindowDrawer
    {
        public override string Title => "Assets";
        private int root;

        private string currentPath;
        private string[] folders;
        private string[] files;

        protected override void OnDraw()
        {
            if(!EditorProject.IsProjectOpen)
            {
                ImGui.Text("No project is open...");
                return;
            }

            if (string.IsNullOrEmpty(currentPath))
            {
                currentPath = GetCurrentRoot();
                UpdateFiles();
            }

            for (int i = 0; i < folders.Length; i++)
            {
                ImGui.Text(folders[i]);
            }
        }

        private void UpdateFiles()
        {
            folders = Directory.GetDirectories(currentPath);
            files = Directory.GetFiles(currentPath);
        }

        private string GetCurrentRoot()
        {
            return root switch
            {
                1 => EditorProject.CurrentProjectPath + "\\" + EditorProject.editorAssetsFolder,
                _ => EditorProject.CurrentProjectPath + "\\" + EditorProject.userAssetsFolder,
            };
        }
    }
}

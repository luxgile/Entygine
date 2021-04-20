using Entygine.DevTools;
using System.IO;
using System.Text.Json;

namespace Entygine_Editor
{
    public static class EditorProject
    {
        private static readonly string projectFileName = "project.econfig";
        private static readonly string assetFolder = "Assets";

        public static void CreateProject(string projectPath, string name)
        {
            if (!Directory.Exists(projectPath))
            {
                DevConsole.Log(LogType.Error, "Path for project not found.");
                return;
            }

            if (Directory.GetFiles(projectPath).Length > 0)
            {
                DevConsole.Log(LogType.Error, "Target folder is not empty.");
                return;
            }

            ProjectData newProject = new ProjectData(name, "0.0.1");
            string json = JsonSerializer.Serialize(newProject);
            File.WriteAllText(projectPath + projectFileName, json);

            ValidateProjectFolders(projectPath);
        }

        public static void ValidateProjectFolders(string projectPath)
        {
            if(!Directory.Exists(projectPath))
            {
                DevConsole.Log(LogType.Error, "Project path doesn't exists");
                return;
            }

            if(!File.Exists(projectPath + projectFileName))
            {
                DevConsole.Log(LogType.Error, "No project was found on path");
                return;
            }

            if(!Directory.Exists(projectPath + assetFolder))
                Directory.CreateDirectory(projectPath + assetFolder);
        }
    }
}

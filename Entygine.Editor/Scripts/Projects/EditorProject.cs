using Entygine.DevTools;
using System.IO;
using System.Text.Json;

namespace Entygine_Editor
{
    public static class EditorProject
    {
        public static ProjectData CurrentProjectData { get; private set; }
        public static string CurrentProjectPath { get; private set; }
        public static bool IsProjectOpen { get; private set; }

        internal static readonly string projectFileName = "project.econfig";
        internal static readonly string userAssetsFolder = "User Assets";
        internal static readonly string editorAssetsFolder = "Editor Assets";

        /// <summary>
        /// Creates project in given path. Target path must be empty.
        /// </summary>
        public static void CreateProject(string projectPath, string name)
        {
            if (!Directory.Exists(projectPath))
            {
                DevConsole.Log(LogType.Error, $"Path for project not found. {projectPath}");
                return;
            }

            projectPath = projectPath + "/" + name;
            Directory.CreateDirectory(projectPath);

            ProjectData newProject = new ProjectData() { Name = name, Version = "0.0.1" };
            string json = JsonSerializer.Serialize(newProject);
            File.WriteAllText(projectPath + "/" + projectFileName, json);

            ValidateProjectFolders(projectPath);
        }

        /// <summary>
        /// Ensures that obligatory folders inside a project exist
        /// </summary>
        public static void ValidateProjectFolders(string projectPath)
        {
            if (!Directory.Exists(projectPath))
            {
                DevConsole.Log(LogType.Error, "Project path doesn't exists");
                return;
            }

            if (!File.Exists(projectPath + "/" + projectFileName))
            {
                DevConsole.Log(LogType.Error, "No project was found on path");
                return;
            }

            if (!Directory.Exists(projectPath + "/" + userAssetsFolder))
                Directory.CreateDirectory(projectPath + "/" + userAssetsFolder);

            if (!Directory.Exists(projectPath + "/" + editorAssetsFolder))
                Directory.CreateDirectory(projectPath + "/" + editorAssetsFolder);
        }

        /// <summary>
        /// Checks if there is a project in the given path
        /// </summary>
        public static bool IsProject(string projectPath)
        {
            return File.Exists(projectPath + "/" + projectFileName);
        }

        /// <summary>
        /// Opens the project in the given path.
        /// </summary>
        public static void OpenProject(string projectPath)
        {
            if (!File.Exists(projectPath + "/" + projectFileName))
            {
                DevConsole.Log(LogType.Error, "No project was found on path");
                return;
            }

            ValidateProjectFolders(projectPath);
            CurrentProjectPath = projectPath;
            string json = File.ReadAllText(projectPath + "/" + projectFileName);
            ProjectData projectData = JsonSerializer.Deserialize<ProjectData>(json);
            CurrentProjectData = projectData;
            IsProjectOpen = true;

            EngineEditorSettings.Current.ProjMeta.LastProjectOpened = projectPath;
        }
    }
}

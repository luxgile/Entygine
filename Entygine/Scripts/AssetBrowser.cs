using Entygine.DevTools;
using System.IO;

namespace Entygine
{
    public static class AssetBrowser
    {
        public static class Utilities
        {
            private static string projectPath;

            //TODO: Path is still not detected automatically but at least git doesn't cry about changes between computers.
            static Utilities()
            {
                ValidateProjectData();
                ReadProjectData();
            }

            private static void ValidateProjectData()
            {
                if (!File.Exists(GetDataPath() + @"\project_data.txt"))
                    File.CreateText(GetDataPath() + @"\project_data.txt").Dispose();
            }

            private static void ReadProjectData()
            {
                string projectPath = File.ReadAllText(GetDataPath() + @"\project_data.txt");
                if (!string.IsNullOrEmpty(projectPath))
                    Utilities.projectPath = projectPath;
                else
                    DevConsole.Log(LogType.Error, "No project path found. Please write the path to 'Assets' folder in the project in this path: " + GetDataPath() + @"\project_data.txt");
            }

            public static string LocalToAbsolutePath(string localPath) => projectPath + localPath;

            public static string GetDataPath() => AppEngineInfo.GetApplicationDataPath();
        }
    }
}

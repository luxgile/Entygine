using System;
using System.IO;
using Utf8Json;
using Utf8Json.Resolvers;

namespace Entygine_Editor
{
    /// <summary>
    /// Class to access all global settings for the editor. This settings persists along all projects.
    /// </summary>
    public class EngineEditorSettings
    {
        public class ProjectsMeta
        {
            public string LastProjectOpened { get; set; }
        }

        public static EngineEditorSettings Current { get; private set; }

        private const string EDITOR_SETTINGS_PATH = @"\Entygine_Editor\Editor Settings.json";
        private static string GetEditorPath() => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + EDITOR_SETTINGS_PATH;

        public ProjectsMeta ProjMeta { get; private set; } = new ProjectsMeta();

        private static void ValidateEditorSettings()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Entygine_Editor";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Saves the Current editor settings
        /// </summary>
        internal static void SaveEditorSettings()
        {
            ValidateEditorSettings();

            var settings = Current;
            string json = JsonSerializer.ToJsonString(settings);
            using FileStream file = File.Create(GetEditorPath());
            using StreamWriter sw = new StreamWriter(file);
            sw.Write(json);
        }

        /// <summary>
        /// Loads the Current editor settings
        /// </summary>
        internal static void LoadEditorSettings()
        {
            ValidateEditorSettings();

            if (File.Exists(GetEditorPath()))
            {
                string json = File.ReadAllText(GetEditorPath());
                EngineEditorSettings settings = JsonSerializer.Deserialize<EngineEditorSettings>(json, StandardResolver.AllowPrivateExcludeNull);
                if (settings == null)
                    settings = new EngineEditorSettings();
                Current = settings;
            }
            else
            {
                Current = new EngineEditorSettings();
            }
        }
    }
}

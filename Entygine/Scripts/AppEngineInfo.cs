using System;
using System.IO;

namespace Entygine
{
    public static class AppEngineInfo
    {
        public static string GetApplicationDataPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Entygine";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}

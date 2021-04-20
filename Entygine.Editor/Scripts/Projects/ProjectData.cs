using System;

namespace Entygine_Editor
{
    public struct ProjectData
    {
        public ProjectData(string name, string version)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version ?? throw new ArgumentNullException(nameof(version));
        }

        public string Name { get; private set; }
        public string Version { get; private set; }
    }
}

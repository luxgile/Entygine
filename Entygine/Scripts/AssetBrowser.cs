namespace Entygine
{
    public static class AssetBrowser
    {
        public static class Utilities
        {
            //TODO: Automatically find project path
            private const string PROJECT_PATH = @"C:\Users\GuillermoAbajo\Documents\Entygine\Entygine\Assets\";
            public static string LocalToAbsolutePath(string localPath) => PROJECT_PATH + localPath;
        }
    }
}

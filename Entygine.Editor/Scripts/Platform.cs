using System.Diagnostics;
using System.Windows.Forms;

namespace Entygine_Editor
{
    public static class Platform
    {
        public static bool OpenFolderBroswer(out string path)
        {
            using var dialog = new FolderBrowserDialog();
            //dialog.show
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = dialog.SelectedPath;
                return true;
            }

            path = null;
            return false;
        }

        public static void OpenExplorerFolder(string path)
        {
            Process.Start("explorer.exe", path);
        }
    }
}

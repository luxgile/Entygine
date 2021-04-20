using Microsoft.WindowsAPICodePack.Dialogs;

namespace Entygine_Editor
{
    public static class Platform
    {
        public static bool OpenFolderBroswer(out string path)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            //dialog.InitialDirectory = "C:\\Users";
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                path = dialog.FileName;
                return true;
            }

            path = null;
            return false;
        }
    }
}

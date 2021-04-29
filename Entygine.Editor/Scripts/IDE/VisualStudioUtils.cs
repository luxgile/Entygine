using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Entygine_Editor.IDE
{
    public static class VisualStudioUtils
    {
        public static void OpenVS(string path, int line)
        {
            foreach (DTE dte in GetInstances())
            {
                if (dte == null)
                    continue;

                dte.MainWindow.Visible = true;
                dte.ExecuteCommand("File.OpenFile", path);
                dte.ExecuteCommand("Edit.GoTo", line.ToString());
                break;
            }
        }
        private static IEnumerable<DTE> GetInstances()
        {
            IRunningObjectTable rot;
            IEnumMoniker enumMoniker;
            int retVal = GetRunningObjectTable(0, out rot);

            if (retVal == 0)
            {
                rot.EnumRunning(out enumMoniker);

                IntPtr fetched = IntPtr.Zero;
                IMoniker[] moniker = new IMoniker[1];
                while (enumMoniker.Next(1, moniker, fetched) == 0)
                {
                    IBindCtx bindCtx;
                    CreateBindCtx(0, out bindCtx);
                    string displayName;
                    moniker[0].GetDisplayName(bindCtx, null, out displayName);
                    Console.WriteLine("Display Name: {0}", displayName);
                    bool isVisualStudio = displayName.StartsWith("!VisualStudio");
                    if (isVisualStudio)
                    {
                        int result = rot.GetObject(moniker[0], out object dteobj);
                        DTE dte = dteobj as DTE;
                        yield return dte;
                    }
                }
            }
        }

        [DllImport("ole32.dll")]
        private static extern void CreateBindCtx(int reserved, out IBindCtx ppbc);

        [DllImport("ole32.dll")]
        private static extern int GetRunningObjectTable(int reserved, out IRunningObjectTable prot);
    }
}

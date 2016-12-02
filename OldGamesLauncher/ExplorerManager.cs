using System;
using System.Diagnostics;

namespace OldGamesLauncher
{
    internal static class ExplorerManager
    {
        public static bool IsRunning
        {
            get
            {
                var procs = Process.GetProcessesByName("explorer");
                return (procs.Length > 0);
            }
        }

        public static void Kill()
        {
            Process p = new Process();
            p.StartInfo.FileName = "taskkill";
            p.StartInfo.Arguments = "/f /IM explorer.exe";
            p.Start();
        }

        public static void Start()
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\explorer.exe";
            p.Start();
        }

    }
}

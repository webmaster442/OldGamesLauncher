using AppLib.WPF.Dialogs;
using Microsoft.Win32;
using OldGamesLauncher.Properties;
using System;
using System.IO;

namespace OldGamesLauncher
{
    public enum WinCDEmuCommands
    {
        DriverInstall,
        DriverUninstall,
        UnmountImages,
        None,
    }

    internal static class WinCDEmuManager
    {
        public static bool IsPathSetup
        {
            get
            {
                if (!string.IsNullOrEmpty(Settings.Default.WinCDEmuPath))
                {
                    return File.Exists(Settings.Default.WinCDEmuPath);
                }
                return false;
            }
        }

        public static bool SetupPath()
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "WinCDEmu portable|PortableWinCDEmu*.exe";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                Settings.Default.WinCDEmuPath = ofd.FileName;
                Settings.Default.Save();
                return true;
            }
            return false;
        }

        private static void Command(string cmd, string param = null)
        {
            try
            {
                var par = cmd;
                if (param != null) par = cmd + " " + param;

                if (!IsPathSetup)
                {
                    var setup = SetupPath();
                    if (!setup)
                        throw new Exception("You must set the WinCDemu portable path, before using this feature");
                    if (!IsPathSetup)
                        throw new Exception("Not a valid WinCDemu portable path.\nYou must set the WinCDemu portable path, before using this feature");
                }

                var p = new System.Diagnostics.Process();
                p.StartInfo.FileName = Settings.Default.WinCDEmuPath;
                p.StartInfo.Arguments = par;
                p.StartInfo.Verb = "runas";
                p.Start();
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
            }
        }

        public static void MountImage()
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                Command(ofd.FileName);
            }
        }

        public static void Command(WinCDEmuCommands command)
        {
            switch (command)
            {
                case WinCDEmuCommands.None:
                    Command(null, null);
                    break;
                case WinCDEmuCommands.DriverInstall:
                    Command("/install");
                    break;
                case WinCDEmuCommands.DriverUninstall:
                    Command("/uninstall");
                    break;
                case WinCDEmuCommands.UnmountImages:
                    Command("/unmountall");
                    break;
            }
        }
    }
}

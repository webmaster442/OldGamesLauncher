using System;
using System.Diagnostics;

namespace OldGamesLauncher.DataFormat
{
    [Serializable]
    public class Emulator
    {
        public string PlatformName
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public void RunGame(string rom = null)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = Path;
                p.StartInfo.Arguments = string.IsNullOrEmpty(rom) ? "" : rom;
                p.Start();
            }
            catch (Exception ex)
            {

            }
        }
    }
}

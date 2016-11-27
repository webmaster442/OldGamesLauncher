using System;
using System.ComponentModel;
using System.Diagnostics;

namespace OldGamesLauncher.DataFormat
{
    [Serializable]
    public class Emulator : INotifyPropertyChanged
    {
        private string _platform;
        private string _path;

        public string PlatformName
        {
            get { return _platform; }
            set
            {
                _platform = value;
                Notify("PlatformName");
            }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                Notify("Path");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public void RunGame(string rom = null)
        {
            Process p = new Process();
            p.StartInfo.FileName = Path;
            if (!string.IsNullOrEmpty(rom))
            {
                if (!System.IO.File.Exists(rom))
                    throw new Exception("Rom file doesn't exist");
                p.StartInfo.Arguments = rom;
            }
            p.Start();
        }
    }
}

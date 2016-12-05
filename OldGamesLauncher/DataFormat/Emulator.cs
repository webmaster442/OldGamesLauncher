using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace OldGamesLauncher.DataFormat
{
    [Serializable]
    public class Emulator : INotifyPropertyChanged
    {
        private string _platform;
        private string _path;
        private string _arguments;
        private bool _checked;

        [XmlAttribute]
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

        public string Arguments
        {
            get { return _arguments; }
            set
            {
                _arguments = value;
                Notify("Arguments");
            }
        }

        [XmlIgnore]
        public bool IsChecked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                Notify("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void RunGame(Game g = null)
        {
            if (g == null)
            {
                if (string.IsNullOrEmpty(Path))
                    throw new Exception("Platform emulator path not set");
                Process.Start(Path);
            }
            else if (g.Platform == "Windows")
            {
                Process p = new Process();
                p.StartInfo.FileName = g.Path;
                p.Start();
            }
            else
            {

                Process p = new Process();

                if (string.IsNullOrEmpty(Path))
                    throw new Exception("Platform emulator path not set");

                p.StartInfo.FileName = Path;
                if (!string.IsNullOrEmpty(g.Path))
                {
                    if (!System.IO.File.Exists(g.Path))
                        throw new Exception("Rom file doesn't exist");

                    if (string.IsNullOrEmpty(Arguments))
                        p.StartInfo.Arguments = g.Path;
                    else
                        p.StartInfo.Arguments = Arguments + " " + g.Path;
                }
                g.LastStartDate = DateTime.Now;
                g.StartCount += 1;
                p.Start();
            }
        }
    }
}

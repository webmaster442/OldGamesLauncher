using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace OldGamesLauncher.DataFormat
{
    [Serializable]
    public class Game : INotifyPropertyChanged
    {
        private DateTime _laststart;
        private uint _startcount;

        /// <summary>
        /// Game path
        /// </summary>
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Game platform
        /// </summary>
        [XmlAttribute]
        public string Platform
        {
            get;
            set;
        }

        /// <summary>
        /// Game name
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Last start date
        /// </summary>
        public DateTime LastStartDate
        {
            get { return _laststart; }
            set
            {
                _laststart = value;
                Notify("LastStartDate");
            }
        }

        /// <summary>
        /// Start counter
        /// </summary>
        [XmlAttribute]
        public uint StartCount
        {
            get { return _startcount; }
            set
            {
                _startcount = value;
                Notify("StartCount");
            }
        }

        public Game()
        {
            LastStartDate = DateTime.MinValue;
            StartCount = 0;
            Name = "";
            Path = "";
            Platform = "";
        }

        public Game(string path, string platform)
        {
            LastStartDate = DateTime.MinValue;
            StartCount = 0;
            Name = GetName(path);
            Path = path;
            Platform = platform;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Notify(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string GetName(string file)
        {
            var name = System.IO.Path.GetFileName(file);
            var ext = System.IO.Path.GetExtension(file);
            name = name.Replace(ext, "");
            return name;
        }
    }
}

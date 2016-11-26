using System;

namespace OldGamesLauncher.DataFormat
{
    [Serializable]
    public class Game
    {
        public string Path
        {
            get;
            set;
        }

        public string Platform
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}

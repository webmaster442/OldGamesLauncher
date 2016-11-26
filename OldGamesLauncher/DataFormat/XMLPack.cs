using System;

namespace OldGamesLauncher.DataFormat
{
    [Serializable]
    public class XMLPack
    {
        public Game[] Games { get; set; }
        public Emulator[] Emulators { get; set; }
    }
}

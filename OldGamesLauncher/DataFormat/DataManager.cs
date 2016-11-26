using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;

namespace OldGamesLauncher.DataFormat
{
    public class DataManager
    {
        private readonly List<Game> Games;

        /// <summary>
        /// Gets the View that can be binded to a list
        /// </summary>
        public ObservableCollection<Game> View
        {
            get;
            private set;
        }

        public List<Emulator> Emulators
        {
            get;
            private set;
        }

        public DataManager()
        {
            Emulators = new List<Emulator>();
            Games = new List<Game>();
            View = new ObservableCollection<Game>();
        }

        /// <summary>
        /// Load data from a file
        /// </summary>
        /// <param name="file">file to load from</param>
        public void Load(string file)
        {
            using (var compressed = File.OpenRead(file))
            {
                using (var uncompressed = new GZipStream(compressed, CompressionMode.Decompress))
                {
                    var ser = new XmlSerializer(typeof(XMLPack));
                    var xmlpack = (XMLPack)ser.Deserialize(uncompressed);
                    Games.Clear();
                    Emulators.Clear();
                    Games.AddRange(xmlpack.Games);
                    Emulators.AddRange(xmlpack.Emulators);
                    xmlpack = null;
                }
            }
            GC.Collect();
        }

        /// <summary>
        /// Save data to a file
        /// </summary>
        /// <param name="file">file to save into</param>
        public void Save(string file)
        {
            using (var compressed = File.Create(file))
            {
                using (var uncompressed = new GZipStream(compressed, CompressionLevel.Optimal))
                {
                    var ser = new XmlSerializer(typeof(XMLPack));
                    var xmlpack = new XMLPack
                    {
                        Games = Games.ToArray(),
                        Emulators = Emulators.ToArray()
                    };
                    ser.Serialize(uncompressed, xmlpack);
                    xmlpack = null;
                }
            }
            GC.Collect();
        }

        public void Search(string name, params string[] CategorieSearch)
        {
            IEnumerable<Game> items = null;
            var catsearch = false;
            if (CategorieSearch != null)
            {
                if (CategorieSearch.Length > 0) catsearch = true;
            }
            if (string.IsNullOrEmpty(name) && !catsearch) items = Games;
            else if (string.IsNullOrEmpty(name) && catsearch)
            {
                var q = from i in Games
                        where CategorieSearch.Contains(i.Platform)
                        orderby i.Name ascending
                        select i;
                items = q;
            }
            else if (!string.IsNullOrEmpty(name) && !catsearch)
            {
                var q = from i in Games
                        where i.Name.Contains(name)
                        orderby i.Name ascending
                        select i;
                items = q;
            }
            else
            {
                var q = from i in Games
                        where i.Name.Contains(name) &&
                        CategorieSearch.Contains(i.Platform)
                        orderby i.Name ascending
                        select i;
                items = q;
            }

            View.Clear();
            foreach (var item in items) View.Add(item);
        }

        public IEnumerable<string> PlatformNames
        {
            get
            {
                return from i in Emulators
                       orderby i.PlatformName ascending
                       select i.PlatformName;
            }
        }

        public void AddGame(Game g)
        {
            Games.Add(g);
        }

        public void AddGames(IEnumerable<Game> games)
        {
            Games.AddRange(games);
        }
    }
}

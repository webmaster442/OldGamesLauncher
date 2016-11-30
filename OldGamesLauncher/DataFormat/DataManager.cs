﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Serialization;
using AppLib.WPF.Extensions;

namespace OldGamesLauncher.DataFormat
{
    public class DataManager
    {
        private readonly List<Game> Games;
        private string _openedfile;

        /// <summary>
        /// Gets the View that can be binded to a list
        /// </summary>
        public ObservableCollection<Game> View
        {
            get;
            private set;
        }

        /// <summary>
        /// Resets selection on all emulators
        /// </summary>
        public void UnSelectPlatforms()
        {
            foreach (var emu in Emulators)
                emu.IsChecked = false;
        }

        /// <summary>
        /// Emulator list view
        /// </summary>
        public ObservableCollection<Emulator> Emulators
        {
            get;
            private set;
        }

        public DataManager()
        {
            Emulators = new ObservableCollection<Emulator>();
            Games = new List<Game>();
            OrderBy = 0;
            View = new ObservableCollection<Game>();
            Emulators.CollectionChanged += Emulators_CollectionChanged;
        }

        private void Emulators_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Modified = true;
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
            Modified = false;
            _openedfile = file;
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
            Modified = false;
        }

        public void SaveOpenedFile()
        {
            if (!string.IsNullOrEmpty(_openedfile))
            {
                if (File.Exists(_openedfile))
                    Save(_openedfile);
            }
        }

        public int OrderBy
        {
            get;
            set;
        }

        /// <summary>
        /// Searches a game in a collection
        /// </summary>
        /// <param name="name">Part of the game name to search for</param>
        /// <param name="PlatformFilter">filter platforms</param>
        public void Search(string name, bool? invariantcase, string[] PlatformFilter)
        {
            IEnumerable<Game> items = null;
            var catsearch = false;
            if (PlatformFilter != null)
            {
                if (PlatformFilter.Length > 0) catsearch = true;
            }
            if (string.IsNullOrEmpty(name) && !catsearch)
            {
                items = Games;
            }
            else if (string.IsNullOrEmpty(name) && catsearch)
            {
                var q = from i in Games
                        where PlatformFilter.Contains(i.Platform)
                        select i;
                items = q;
            }
            else if (!string.IsNullOrEmpty(name) && !catsearch)
            {
                var q = from i in Games
                        where invariantcase == true ? i.Name.ToLower().Contains(name.ToLower()) : i.Name.Contains(name)
                        select i;
                items = q;
            }
            else
            {
                var q = from i in Games
                        where invariantcase == true ? i.Name.ToLower().Contains(name.ToLower()) : i.Name.Contains(name) &&
                        PlatformFilter.Contains(i.Platform)
                        select i;
                items = q;
            }

            View.Clear();

            switch (OrderBy)
            {
                case 0:
                    items.OrderBy(i => i.Name);
                    break;
                case 1:
                    items.OrderBy(i => i.StartCount).ThenBy(i => i.Name);
                    break;
                case 2:
                    items.OrderBy(i => i.LastStartDate).ThenBy(i => i.Name);
                    break;
            }

            foreach (var item in items) View.Add(item);
        }

        /// <summary>
        /// Add a game to the collection
        /// </summary>
        /// <param name="g">game to add</param>
        public void AddGame(Game g)
        {
            Games.Add(g);
            Modified = true;
        }

        /// <summary>
        /// Add a range of games to the collection
        /// </summary>
        /// <param name="games">games to add</param>
        public void AddGames(IEnumerable<Game> games)
        {
            Games.AddRange(games);
            Modified = true;
        }

        /// <summary>
        /// Gets information about the state of the collection since last save
        /// </summary>
        public bool Modified
        {
            get;
            private set;
        }

        public Emulator GetEmulator(Game g)
        {
            var q = from i in Emulators
                    where i.PlatformName == g.Platform
                    select i;
            return q.FirstOrDefault();
        }
    }
}

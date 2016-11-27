using Microsoft.Win32;
using OldGamesLauncher.DataFormat;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using AppLib.WPF.Extensions;
using System.Linq;
using System;

namespace OldGamesLauncher.Dialogs
{
    /// <summary>
    /// Interaction logic for AddGame.xaml
    /// </summary>
    public partial class AddGame : UserControl, IDialog
    {
        private string[] _selected;

        public AddGame()
        {
            InitializeComponent();
            App.DataMan.UnSelectPlatforms();
            PlatformSelector.ItemsSource = App.DataMan.Emulators;
        }

        private void AddFiles_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            ofd.Title = "Add Games...";
            ofd.Filter = "All files|*.*";
            if (ofd.ShowDialog() == true)
            {
                _selected = ofd.FileNames;
                ItemCounter.Text = string.Format("{0} File(s) selected", _selected.Length);
            }
        }

        private string GetName(string file)
        {
            var name = Path.GetFileName(file);
            var ext = Path.GetExtension(file);
            name = name.Replace(ext, "");
            return name;
        }

        private string GetSelectedPlatform()
        {
            var q = from i in App.DataMan.Emulators
                    where i.IsChecked == true
                    select i.PlatformName;
            return q.FirstOrDefault();
        }


        public IEnumerable<Game> Items
        {
            get
            {
                foreach (var file in _selected)
                {
                    yield return new Game
                    {
                        Path = file,
                        Name = GetName(file),
                        Platform = GetSelectedPlatform()
                    };
                }
            }
        }

        public Action OkAction
        {
            get;
            set;
        }
    }
}

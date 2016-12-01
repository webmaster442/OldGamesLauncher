using Microsoft.Win32;
using OldGamesLauncher.Properties;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace OldGamesLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void OpenDialog(UserControl ctrl, string title)
        {
            DialogContainer.DialogContent = ctrl;
            DialogContainer.Title = title;
            DialogContainer.Open();
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Old Games Launcher databases|*.ogdb";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                App.DataMan.Load(ofd.FileName);
                Settings.Default.LastFile = ofd.FileName;
            }
        }

        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Old Games Launcher databases|*.ogdb";
            if (sfd.ShowDialog() == true)
            {
                App.DataMan.Save(sfd.FileName);
                Settings.Default.LastFile = sfd.FileName;
            }
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void GamesAdd_Click(object sender, RoutedEventArgs e)
        {
            var add = new Dialogs.AddGame();
            add.OkAction = new System.Action(() =>
            {
                App.DataMan.AddGames(add.Items);
            });
            OpenDialog(add, "Add games...");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Settings.Default.LastFile))
            {
                App.DataMan.Load(Settings.Default.LastFile);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (App.DataMan.Modified)
            {
                var msg = MessageBox.Show("Collection contains unsaved changes. Save now?",
                                          "Unsaved Changes",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);
                if (msg == MessageBoxResult.Yes) FileSave_Click(this, null);
            }
            else App.DataMan.SaveOpenedFile();
            Settings.Default.Save();
        }

        private void StartURL(string url, string param = null)
        {
            var tostart = url + param;
            System.Diagnostics.Process.Start(tostart);
        }

        private void IntenetSearch_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as MenuItem).Name;
            var gamedata = string.Format("{0} {1}", 
                                         GamesBrowser.SelectedItem.Name,
                                         GamesBrowser.SelectedItem.Platform);
            switch (s)
            {
                case "InternetGoogle":
                    StartURL("https://www.google.hu/search?q=", gamedata);
                    break;
                case "InternetDuckDuck":
                    StartURL("https://duckduckgo.com/?q=", gamedata);
                    break;
                case "InternetSearchCheat":
                    StartURL("https://duckduckgo.com/?q=", gamedata + " cheats");
                    break;
            }

        }

        private void InternetDloadEmu_Click(object sender, RoutedEventArgs e)
        {
            StartURL("http://www.emuparadise.me/Emulators.php");
        }

        private void IntenetVisitProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GamesDeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            App.DataMan.DeleteSelection(GamesBrowser.SelectedItems);
        }

        private void GamesDeleteWithoutEmulator_Click(object sender, RoutedEventArgs e)
        {
            App.DataMan.DeleteGamesWithoutEmulator();
        }

        private void ROMS_Click(object sender, RoutedEventArgs e)
        {
            var s = (sender as MenuItem).Header.ToString();
            switch (s)
            {
                case "Emuparadise":
                    StartURL("http://www.emuparadise.me/roms-isos-games.php");
                    break;
                case "Coolrom":
                    StartURL("http://coolrom.com/");
                    break;
                case "Loveroms":
                    StartURL("https://www.loveroms.com/");
                    break;
                case "FreeRoms":
                    StartURL("http://www.freeroms.com/");
                    break;
            }
        }
    }
}

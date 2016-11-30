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
    }
}

using AppLib.WPF.Dialogs;
using OldGamesLauncher.DataFormat;
using OldGamesLauncher.Styles;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OldGamesLauncher
{
    /// <summary>
    /// Interaction logic for Games.xaml
    /// </summary>
    public partial class Games : UserControl
    {
        private bool _loaded;

        public Games()
        {
            InitializeComponent();
            PlatformFilter.DataContext = this;
            CheckedCommand = new RelayCommand(o => { Checked(o); }, o => true);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
            LbView.ItemsSource = App.DataMan.View;
            App.DataMan.UnSelectPlatforms();
            PlatformFilter.ItemsSource = App.DataMan.Emulators;
            App.DataMan.Search("", null);
        }

        private void ViewList_Checked(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            LbView.Style = FindResource("ListboxList") as Style;
        }

        private void ViewGrid_Checked(object sender, RoutedEventArgs e)
        {
            if (!_loaded) return;
            LbView.Style = FindResource("ListboxGrid") as Style;
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            App.DataMan.Search(TbSearch.Text, null); ;
        }

        public ICommand CheckedCommand { get; set; }

        private void Checked(object sender)
        {
            var selected = from i in App.DataMan.Emulators
                           where i.IsChecked == true
                           select i.PlatformName;
            App.DataMan.Search(TbSearch.Text, selected.ToArray());
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbSearch.Text = "";
        }

        private void LbView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (LbView.SelectedItem == null) return;
                var game = LbView.SelectedItem as Game;
                var emu = App.DataMan.GetEmulator(game);
                if (emu == null)
                    throw new Exception("No emulator found for platform: " + game.Platform);
                emu.RunGame(game.Path);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
            }
        }
    }
}

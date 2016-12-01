using AppLib.WPF.Dialogs;
using OldGamesLauncher.DataFormat;
using OldGamesLauncher.Styles;
using System;
using System.Collections.Generic;
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
        private int _sort;

        public Games()
        {
            InitializeComponent();
            PlatformFilter.DataContext = this;
            CheckedCommand = new RelayCommand(o => { Checked(o); }, o => true);
        }

        public ICommand CheckedCommand { get; set; }

        public static readonly DependencyProperty IsItemSelectedProperty =
            DependencyProperty.Register("IsItemSelected", typeof(bool), typeof(Games), new PropertyMetadata(false));

        public bool IsItemSelected
        {
            get { return (bool)GetValue(IsItemSelectedProperty); }
            set { SetValue(IsItemSelectedProperty, value); }
        }

        public Game SelectedItem
        {
            get { return LbView.SelectedItem as Game; }
        }

        public IEnumerable<Game> SelectedItems
        {
            get
            {
                foreach (var item in LbView.SelectedItems)
                {
                    yield return item as Game;
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
            LbView.ItemsSource = App.DataMan.View;
            App.DataMan.UnSelectPlatforms();
            PlatformFilter.ItemsSource = App.DataMan.Emulators;
            App.DataMan.Search("", true, null);
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

        private void BtnSort_Click(object sender, RoutedEventArgs e)
        {
            if (_sort + 1 > 2) _sort = 0;
            else _sort += 1;
            switch (_sort)
            {
                case 0:
                    BtnSort.Content = "Name";
                    break;
                case 1:
                    BtnSort.Content = "Start count";
                    break;
                case 2:
                    BtnSort.Content = "Last start date";
                    break;
            }
            App.DataMan.OrderBy = (OrderKind)_sort;
        }

        private void Search()
        {
            var selected = from i in App.DataMan.Emulators
                           where i.IsChecked == true
                           select i.PlatformName;
            App.DataMan.Search(TbSearch.Text, CbInvariant.IsChecked, selected.ToArray());
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Search();
        }

        private void Checked(object sender)
        {
            Search();
        }

        private void CbInvariant_Checked(object sender, RoutedEventArgs e)
        {
            Search();
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
                emu.RunGame(game);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
            }
        }

        private void LbView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            IsItemSelected = LbView.SelectedItems.Count > 0;
        }
    }
}

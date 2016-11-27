using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            LbView.ItemsSource = App.DataMan.View;
            App.DataMan.UnSelectPlatforms();
            PlatformFilter.ItemsSource = App.DataMan.Emulators;
            App.DataMan.Search("", null);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
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

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            TbSearch.Text = "";
        }
    }
}

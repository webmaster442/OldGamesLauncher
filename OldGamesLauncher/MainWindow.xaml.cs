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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public static void OpenDialog(UserControl content)
        {
            var mw = App.Current.MainWindow as MainWindow;
            mw.Dialog.Visibility = Visibility.Visible;
            mw.DialogContent.Content = content;
        }

        private void DialogClose_Click(object sender, RoutedEventArgs e)
        {
            Dialog.Visibility = Visibility.Collapsed;
        }
    }
}

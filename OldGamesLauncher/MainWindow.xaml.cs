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
using Microsoft.Win32;

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

        private void FileOpen_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Old Games Launcher databases|*.ogdb";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                App.DataMan.Load(ofd.FileName);
            }
        }

        private void FileSave_Click(object sender, RoutedEventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "Old Games Launcher databases|*.ogdb";
            if (sfd.ShowDialog() == true)
            {
                App.DataMan.Load(sfd.FileName);
            }
        }

        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

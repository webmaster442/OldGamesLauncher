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

namespace OldGamesLauncher.Dialogs
{
    /// <summary>
    /// Interaction logic for StartWithDialog.xaml
    /// </summary>
    public partial class StartWithDialog : UserControl
    {
        public StartWithDialog()
        {
            InitializeComponent();
            EmuStarter.ItemsSource = App.DataMan.Emulators;
        }
    }
}

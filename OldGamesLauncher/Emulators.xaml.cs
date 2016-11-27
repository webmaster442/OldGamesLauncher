using OldGamesLauncher.DataFormat;
using OldGamesLauncher.Styles;
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
    /// Interaction logic for Emulators.xaml
    /// </summary>
    public partial class Emulators : UserControl
    {
        public Emulators()
        {
            InitializeComponent();
            EmulatorList.DataContext = this;
            DeleteCommand = new RelayCommand(o => { Delete(o); }, o => true);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            EmulatorList.ItemsSource = App.DataMan.Emulators;
        }

        private void BtnAddEmu_Click(object sender, RoutedEventArgs e)
        {
            App.DataMan.Emulators.Add(new Emulator());
        }

        public ICommand DeleteCommand { get; set; }

        private void Delete(object sender)
        {
            var emu = sender as Emulator;
            App.DataMan.Emulators.Remove(emu);
        }

    }
}

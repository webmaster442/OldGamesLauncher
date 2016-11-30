using AppLib.WPF.Dialogs;
using OldGamesLauncher.DataFormat;
using OldGamesLauncher.Styles;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            RunCommand = new RelayCommand(o => { Run(o); }, o => true);
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
        public ICommand RunCommand { get; set; }

        private void Delete(object sender)
        {
            var emu = sender as Emulator;
            App.DataMan.Emulators.Remove(emu);
        }

        private void Run(object o)
        {
            try
            {
                var emu = o as Emulator;
                emu.RunGame(null);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
            }
        }
    }
}

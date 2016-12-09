using AppLib.WPF.Dialogs;
using OldGamesLauncher.DataFormat;
using OldGamesLauncher.Styles;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace OldGamesLauncher.Dialogs
{
    /// <summary>
    /// Interaction logic for StartWithDialog.xaml
    /// </summary>
    public partial class StartWithDialog : UserControl, IDialog
    {
        private IDialogHost _host;

        public StartWithDialog(IDialogHost host)
        {
            InitializeComponent();
            _host = host;
            EmuStarter.DataContext = this;
            StartCommand = new RelayCommand(o => { Start(o); }, o => true);
            EmuStarter.ItemsSource = App.DataMan.Emulators;
        }

        public IDialogHost Host
        {
            get { return _host; }
        }

        public Action OkAction
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Host.Close();
        }

        public ICommand StartCommand { get; set; }

        private void BtnWinShell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(FileName);
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
            }
            Host.Close();
        }

        private void Start(object o)
        {
            try
            {
                var emu = o as Emulator;
                emu.RunGame(new Game() { Path = FileName, Platform = emu.PlatformName });
            }
            catch (Exception ex)
            {
                ErrorDialog.Show(ex);
            }
            Host.Close();
        }
    }
}

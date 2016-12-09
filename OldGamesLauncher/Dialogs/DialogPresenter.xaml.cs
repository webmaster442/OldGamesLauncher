using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace OldGamesLauncher.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogPresenter.xaml
    /// </summary>
    public partial class DialogPresenter : UserControl, IDialogHost
    {
        public DialogPresenter()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(DialogPresenter), new PropertyMetadata(""));

        public static readonly DependencyProperty BtnOkLabelProperty =
            DependencyProperty.Register("BtnOkLabel", typeof(string), typeof(DialogPresenter), new PropertyMetadata("Ok"));

        public static readonly DependencyProperty DialogContentProperty =
            DependencyProperty.Register("DialogContent", typeof(FrameworkElement), typeof(DialogPresenter));

        public static readonly DependencyProperty DialogControlsVisibilityProperty =
            DependencyProperty.Register("DialogControlsVisibility", typeof(Visibility), typeof(DialogPresenter), new PropertyMetadata(Visibility.Visible));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public string BtnOkLabel
        {
            get { return (string)GetValue(BtnOkLabelProperty); }
            set { SetValue(BtnOkLabelProperty, value); }
        }

        public FrameworkElement DialogContent
        {
            get { return (FrameworkElement)GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }

        public Visibility DialogControlsVisibility
        {
            get { return (Visibility)GetValue(DialogControlsVisibilityProperty); }
            set { SetValue(DialogControlsVisibilityProperty, value); }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            var dialog = DialogContent as IDialog;
            if (dialog != null)
            {
                dialog.OkAction?.Invoke();
            } 
            Close();
        }

        public void Open()
        {
            Visibility = Visibility.Visible;
            var open = FindResource("Open") as Storyboard;
            BeginStoryboard(open);
        }

        public void Close()
        {
            var close = FindResource("Close") as Storyboard;
            close.Completed += Close_Completed;
            BeginStoryboard(close);
        }

        private void Close_Completed(object sender, System.EventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }

    public interface IDialogHost
    {
        void Close();
    }

    public interface IDialog
    {
        Action OkAction { get; set; }
        IDialogHost Host { get; }
    }
}

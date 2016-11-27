using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace OldGamesLauncher.Dialogs
{
    /// <summary>
    /// Interaction logic for DialogPresenter.xaml
    /// </summary>
    public partial class DialogPresenter : UserControl
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

        public event RoutedEventHandler OkClick;

        public event RoutedEventHandler CancelClick;

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (OkClick != null) OkClick(sender, e);
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            if (CancelClick != null) CancelClick(sender, e);
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
}

using System.Windows;
using WpfClient.Services;

namespace WpfClient.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AppNavHelper.NavigationService = NavigationFrame.NavigationService;
            AppNavHelper.ShowProgressBar = () => ProgressBar.Visibility = Visibility.Visible;
            AppNavHelper.HideProgressBar = () => ProgressBar.Visibility = Visibility.Hidden;
            // Start page
            AppNavHelper.NavigationService.Navigate(new AuthorizationView());
        }
    }
}

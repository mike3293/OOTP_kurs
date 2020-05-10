using System;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using WpfClient.Services;

namespace WpfClient.Views
{
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

            AppNavHelper.Notifier =  new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
            // Start page
            AppNavHelper.NavigationService.Navigate(new AuthorizationView());
        }
    }
}

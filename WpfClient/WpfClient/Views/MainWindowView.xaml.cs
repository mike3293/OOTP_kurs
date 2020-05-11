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
            AppNavHelper appNavHelper = AppNavHelper.GetInstance();
            appNavHelper.NavigationService = NavigationFrame.NavigationService;
            appNavHelper.ShowProgressBar = () => ProgressBar.Visibility = Visibility.Visible;
            appNavHelper.HideProgressBar = () => ProgressBar.Visibility = Visibility.Hidden;

            appNavHelper.Notifier =  new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
            // Start page
            appNavHelper.NavigationService.Navigate(new AuthorizationView());
        }
    }
}

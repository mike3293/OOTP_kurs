using System;
using System.Windows;
using System.Windows.Navigation;
using ToastNotifications;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public static class AppNavHelper
    {
        public static NavigationService NavigationService { set; get; }

        public static User CurrentUser { set; get; }

        public static Action ShowProgressBar { set; get; }

        public static Action HideProgressBar { set; get; }

        public static Notifier Notifier { set; get; }
    }
}

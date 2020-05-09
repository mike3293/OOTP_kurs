using System;
using System.Windows;
using System.Windows.Navigation;

namespace WpfClient.Services
{
    public static class AppNavHelper
    {
        public static NavigationService NavigationService { set; get; }

        public static Action ShowProgressBar { set; get; }

        public static Action HideProgressBar { set; get; }
    }
}

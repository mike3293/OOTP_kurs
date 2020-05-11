using System;
using System.Windows.Navigation;
using ToastNotifications;
using WpfClient.DataBase.Models;

namespace WpfClient.Services
{
    public class AppNavHelper
    {
        private static readonly Lazy<AppNavHelper> lazy = new Lazy<AppNavHelper>(() => new AppNavHelper());

        public string Name { get; private set; }

        private AppNavHelper()
        {
        }

        public static AppNavHelper GetInstance()
        {
            return lazy.Value;
        }

        public NavigationService NavigationService { set; get; }

        public User CurrentUser { set; get; }

        public Action ShowProgressBar { set; private get; }

        public Action HideProgressBar { set; private get; }

        public Notifier Notifier { set; get; }


        private object _locker = new object ();

        private int _taskCounter = 0;

        public void IncrementTasksCounter()
        {
            lock (_locker)
            {
                if (_taskCounter == 0)
                {
                    ShowProgressBar();
                }
                _taskCounter++;
            }
        }

        public void DecrementTasksCounter()
        {
            lock (_locker)
            {
                _taskCounter = _taskCounter > 0 ? --_taskCounter : 0;
                if (_taskCounter == 0)
                {
                    HideProgressBar();
                }
            }
        }

        public bool CheckIfNoTasks()
        {
            lock (_locker)
            {
                return _taskCounter == 0;
            }
        }
    }
}

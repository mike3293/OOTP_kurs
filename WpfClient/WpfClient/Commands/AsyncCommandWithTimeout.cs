using Nito.AsyncEx;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ToastNotifications.Messages;
using WpfClient.Services;

namespace WpfClient.Commands
{
    public class AsyncCommandWithTimeout : IAsyncCommand
    {
        private readonly Func<object, Task> _command;
        private Func<object, bool> canExecute;

        public AsyncCommandWithTimeout(Func<object, Task> command, Func<object, bool> canExecute = null)
        {
            _command = command;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public Task ExecuteAsync(object parameter)
        {
            return _command(parameter);
        }

        public async void Execute(object parameter)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(10);

            try
            {
                CancellationTokenSource cts = new CancellationTokenSource(timeout);
                await ExecuteAsync(parameter).WaitAsync(cts.Token);
            }
            catch (Exception e)
            {
                AppNavHelper.HideProgressBar();
                AppNavHelper.Notifier.ShowError(e.Message);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

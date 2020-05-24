using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications.Messages;
using WpfClient.Services;

namespace WpfClient.Commands
{
    public class AsyncCommand : IAsyncCommand
    {
        private AppNavHelper _appNavHelper = AppNavHelper.GetInstance();
        private readonly Func<object, Task> _command;
        private Func<object, bool> canExecute;

        public AsyncCommand(Func<object, Task> command, Func<object, bool> canExecute = null)
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
            try
            {
                _appNavHelper.IncrementTasksCounter();

                await ExecuteAsync(parameter);
            }
            catch (Exception e)
            {
                _appNavHelper.Notifier.ShowError(e.Message);
            }
            finally
            {
                _appNavHelper.DecrementTasksCounter();
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

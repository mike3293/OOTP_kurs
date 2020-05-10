﻿using Nito.AsyncEx;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfClient.Services;

namespace WpfClient.Commands
{
    internal class AsyncCommand : IAsyncCommand
    {
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
            await ExecuteAsync(parameter);
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

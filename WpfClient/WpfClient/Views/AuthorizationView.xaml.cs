using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfClient.Auth;
using WpfClient.DataBase;
using WpfClient.Models;
using WpfClient.ViewModels;

namespace WpfClient.Views
{
    public partial class AuthorizationView : Page
    {
        private readonly HashSet<ValidationError> errors = new HashSet<ValidationError>();
        private AuthorizationViewModel viewModel;

        public AuthorizationView()
        {
            InitializeComponent();
            InitializeValidaton();
        }

        void InitializeValidaton()
        {
            viewModel = new AuthorizationViewModel();
            Validation.AddErrorHandler(this, ErrorChangedHandler);
            DataContext = viewModel;
        }

        private void ErrorChangedHandler(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                errors.Add(e.Error);
            }
            else
            {
                errors.Remove(e.Error);
            }
            
            viewModel.IsValid = !errors.Any();
        }
    }
}

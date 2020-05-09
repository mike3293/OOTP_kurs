﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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

        private void InitializeValidaton()
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfClient.ViewModels;

namespace WpfClient.Validations
{
    public abstract class PageWithValidation : Page
    {
        private readonly HashSet<ValidationError> errors = new HashSet<ValidationError>();
        private ViewModelBase viewModel;

        public PageWithValidation(ViewModelBase viewModel)
        {
            InitializeValidaton(viewModel);
        }

        void InitializeValidaton(ViewModelBase viewModel)
        {
            this.viewModel = viewModel;
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

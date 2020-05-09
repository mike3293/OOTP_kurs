using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfClient.ViewModels;

namespace WpfClient.Views
{
    public partial class ManagerView : Page
    {
        private readonly HashSet<ValidationError> errors = new HashSet<ValidationError>();
        private ManagerViewModel viewModel;

        public ManagerView()
        {
            InitializeComponent();
            InitializeValidaton();
        }

        private void InitializeValidaton()
        {
            viewModel = new ManagerViewModel();
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

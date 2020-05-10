using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfClient.Services;
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




        //private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    Internships.SelectedItems.Clear();

        //    ListViewItem item = sender as ListViewItem;
        //    if (item != null)
        //    {
        //        item.IsSelected = true;
        //        Internships.SelectedItem = item;
        //    }
        //}

        //private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    ListViewItem item = sender as ListViewItem;
        //    if (item != null && item.IsSelected)
        //    {
        //        AppNavHelper.NavigationService.Navigate(new InternView());
        //    }
        //}
    }

}

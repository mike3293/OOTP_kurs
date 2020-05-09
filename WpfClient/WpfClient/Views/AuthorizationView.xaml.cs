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

        //private void OnSignIn(object sender, RoutedEventArgs e)
        //{
        //    NavigationService.Navigate(new Uri("MainView.xaml", UriKind.Relative));
        //}

        //private void OnSignUp(object sender, RoutedEventArgs e)
        //{
        //    Person person = new Person
        //    {
        //        FirstName = "First",
        //        LastName = "Last",
        //    };

        //    User user = new User
        //    {
        //        Email = "manager@manager.ru",
        //        HashedPassword = PasswordEncoder.GetHash("manager"),
        //        Role = Role.Manager,
        //        UserDetails = person,
        //    };

        //    DataBaseContext context = new DataBaseContext();
        //    context.Users.Add(user);
        //    context.SaveChangesAsync();

        //    //Internship i = new Internship
        //    //{
        //    //    Id = Guid.NewGuid(),
        //    //};

        //    //BackEndContext context = new BackEndContext();
        //    //context.Internships.Add(i);
        //    //context.SaveChanges();
        //}
    }
}

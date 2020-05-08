using System;
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
        public AuthorizationView()
        {
            InitializeComponent();

            DataContext = new AuthorizationViewModel();
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

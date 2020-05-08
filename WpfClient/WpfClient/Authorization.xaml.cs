using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using WpfClient.BackEnd.DataBase;

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void OnSignIn(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("MainView.xaml", UriKind.Relative));
        }

        private void OnSignUp(object sender, RoutedEventArgs e)
        {
            Person person = new Person
            {
                Id = Guid.NewGuid(),
                Email = "manager@intern.ru",
                FirstName = "First",
                LastName = "Last",
                Role = Role.Manager,
            };

            BackEndContext context = new BackEndContext();
            context.People.Add(person);
            context.SaveChanges();

            //Internship i = new Internship
            //{
            //    Id = Guid.NewGuid(),
            //};

            //BackEndContext context = new BackEndContext();
            //context.Internships.Add(i);
            //context.SaveChanges();
        }
    }
}

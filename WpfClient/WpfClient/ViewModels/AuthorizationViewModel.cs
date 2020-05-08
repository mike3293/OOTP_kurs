using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using WpfClient.Auth;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Models;
using WpfClient.Services;

namespace WpfClient.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase
    {
        public UserCredentials UserCredentials = new UserCredentials();

        public string Email
        {
            get => UserCredentials.Email;
            set
            {
                UserCredentials.Email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Password
        {
            get => UserCredentials.Password;
            set
            {
                UserCredentials.Password = value;
                OnPropertyChanged("Password");
            }
        }

        #region SignInCommand

        private Command _signInCommand;

        public ICommand SignInCommand => _signInCommand ?? (_signInCommand = new Command(
                obj =>
                {
                    User user = UsersService.GetUserByEmail(Email);

                    if (user != null)
                    {
                        string hashedPassword = PasswordEncoder.GetHash((obj as PasswordBox).Password);

                        if (hashedPassword.Equals(user.HashedPassword))
                        {
                            // TODO
                            AppNavHelper.NavigationService.Navigate(new MainView());
                        }
                    }
                }));
        #endregion
    }
}

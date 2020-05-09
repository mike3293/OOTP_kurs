using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfClient.Auth;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Models;
using WpfClient.Services;

namespace WpfClient.ViewModels
{
    public class SignUpViewModel : ViewModelBase
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

        #region Validation
        private bool _isValid;
        public bool IsValid
        {
            get { return _isValid; }
            set
            {
                _isValid = value;
                OnPropertyChanged("IsValid");
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }
        #endregion

        #region SignInCommand

        private AsyncCommand _signInCommand;

        public IAsyncCommand SignInCommand => _signInCommand ?? (_signInCommand = new AsyncCommand(
                async (obj) =>
                {
                    string hashedPassword = PasswordEncoder.GetHash(Password);

                    AppNavHelper.ShowProgressBar();
                    User user = await Task.Run(() => UsersService.GetUserByEmail(Email));
                    AppNavHelper.HideProgressBar();

                    if (user != null)
                    {
                        if (hashedPassword.Equals(user.HashedPassword))
                        {
                            AppNavHelper.NavigationService.Navigate(new MainView());
                            ErrorMessage = null;
                            return;
                        }
                        ErrorMessage = "Password incorrect";
                        return;
                    }
                    ErrorMessage = "User not found";
                }, (obj) => IsValid));

        #endregion
    }
}
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClient.Auth;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Models;
using WpfClient.Services;
using WpfClient.Views;

namespace WpfClient.ViewModels
{
    public class AuthorizationViewModel : ViewModelBase
    {
        // TODO: Delete init
        public UserCredentials UserCredentials = new UserCredentials() { Email = "manager@manager.ru" };

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
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged("IsValid");
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
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
                    User user = await Task.Run(() => UsersService.GetUserByEmailAsync(Email));
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

        #region SignUpCommand

        private Command _signUpCommand;

        public ICommand SignUpCommand => _signUpCommand ?? (_signUpCommand = new Command(
                (obj) =>
                {
                    AppNavHelper.NavigationService.Navigate(new SignUpView());
                    ErrorMessage = null;
                    return;
                }));

        #endregion
    }
}
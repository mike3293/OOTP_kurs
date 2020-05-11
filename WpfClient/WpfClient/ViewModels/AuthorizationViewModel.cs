using System.Threading.Tasks;
using System.Windows.Controls;
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
        public UserCredentials UserCredentials = new UserCredentials() { Email = "manager", Password="manager" };

        public string Email
        {
            get => UserCredentials.Email;
            set
            {
                UserCredentials.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Password
        {
            get => UserCredentials.Password;
            set
            {
                UserCredentials.Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        #region Validation
        private bool _isValid = true;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        #endregion

        #region SignInCommand

        private AsyncCommandWithTimeout _signInCommand;

        public IAsyncCommand SignInCommand => _signInCommand ?? (_signInCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    AppNavHelper appNavHelper = AppNavHelper.GetInstance();
                    string hashedPassword = PasswordEncoder.GetHash(Password);

                    appNavHelper.IncrementTasksCounter();
                    User user = await Task.Run(() => UsersService.GetUserByEmailAsync(Email));
                    appNavHelper.DecrementTasksCounter();

                    if (user != null)
                    {
                        if (hashedPassword.Equals(user.HashedPassword))
                        {
                            appNavHelper.CurrentUser = user;
                            NavigateByUserRole(user);
                            ErrorMessage = null;
                            return;
                        }
                        ErrorMessage = "Password incorrect";
                        return;
                    }
                    ErrorMessage = "User not found";
                }, (obj) => IsValid));

        private void NavigateByUserRole(User user)
        {
            Page view;
            switch (user.Role)
            {
                case Role.Manager: view = new ManagerView(); break;
                case Role.Intern: view = new InternView(user); break;
                default: view = new NotUpprovedView(); ; break;
            }
            AppNavHelper.GetInstance().NavigationService.Navigate(view);
        }

        #endregion

        #region SignUpCommand

        private Command _signUpCommand;

        public ICommand SignUpCommand => _signUpCommand ?? (_signUpCommand = new Command(
                (obj) =>
                {
                    AppNavHelper.GetInstance().NavigationService.Navigate(new SignUpView());
                    ErrorMessage = null;
                    return;
                }));

        #endregion
    }
}
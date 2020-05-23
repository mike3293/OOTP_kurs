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
        private AppNavHelper _appNavHelper = AppNavHelper.GetInstance();
        // TODO: Delete init
        public UserCredentials UserCredentials = new UserCredentials() { Email = "manager@gmail.com", Password = "manager@gmail.com" };

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

        private AsyncCommand _signInCommand;

        public IAsyncCommand SignInCommand => _signInCommand ?? (_signInCommand = new AsyncCommand(
                async (obj) =>
                {
                    User user = await Task.Run(() => UsersService.GetUserByEmailAsync(Email));

                    if (user != null)
                    {
                        if (PasswordEncoder.Verify(user.HashedPassword, user.Email, Password))
                        {
                            _appNavHelper.CurrentUser = user;
                            NavigateByUserRole(user);
                            ErrorMessage = null;
                            return;
                        }
                        ErrorMessage = "Password incorrect";
                        return;
                    }
                    ErrorMessage = "User not found";
                }, (obj) => IsValid && _appNavHelper.CheckIfNoTasks()));

        private void NavigateByUserRole(User user)
        {
            Page view;
            switch (user.Role)
            {
                case Role.Manager: view = new ManagerView(); break;
                case Role.Intern: view = new InternView(user); break;
                default: view = new NotUpprovedView(); ; break;
            }
            _appNavHelper.NavigationService.Navigate(view);
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
                }, (obj) => _appNavHelper.CheckIfNoTasks()));

        #endregion
    }
}
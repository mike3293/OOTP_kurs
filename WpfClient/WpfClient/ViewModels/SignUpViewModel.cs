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
    public class SignUpViewModel : ViewModelBase
    {
        private AppNavHelper _appNavHelper = AppNavHelper.GetInstance();

        public UserSignUpCredentials UserCredentials = new UserSignUpCredentials();

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

        public string FirstName
        {
            get => UserCredentials.FirstName;
            set
            {
                UserCredentials.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => UserCredentials.LastName;
            set
            {
                UserCredentials.LastName = value;
                OnPropertyChanged(nameof(LastName));
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

        #region SignUpCommand

        private AsyncCommand _signUpCommand;

        public IAsyncCommand SignUpCommand => _signUpCommand ?? (_signUpCommand = new AsyncCommand(
                async (obj) =>
                {
                    string hashedPassword = PasswordEncoder.GetHash(Password, Email);

                    bool emailExists = await Task.Run(() => UsersService.CheckIfUserExistsByEmailAsync(Email));

                    if (emailExists)
                    {
                        ErrorMessage = "Email already registered";
                        return;
                    }

                    Person person = new Person
                    {
                        FirstName = FirstName,
                        LastName = LastName,
                    };

                    User user = new User
                    {
                        Email = Email,
                        HashedPassword = hashedPassword,
                        UserDetails = person,
                    };

                    bool userCreated = await Task.Run(() => UsersService.AddUserAsync(user));
                    if (userCreated)
                    {
                        _appNavHelper.NavigationService.Navigate(new AuthorizationView());
                        ErrorMessage = null;
                    }
                    else
                    {
                        ErrorMessage = "User was not created";
                    }
                }, (obj) => IsValid && _appNavHelper.CheckIfNoTasks()));

        #endregion

        #region BackCommand

        private Command _backCommand;

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(
                (obj) =>
                {
                    _appNavHelper.NavigationService.Navigate(new AuthorizationView());
                    ErrorMessage = null;
                }, (obj) => _appNavHelper.CheckIfNoTasks()));

        #endregion
    }
}
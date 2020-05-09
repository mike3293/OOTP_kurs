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
        public UserSignUpCredentials UserCredentials = new UserSignUpCredentials();

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

        public string FirstName
        {
            get => UserCredentials.FirstName;
            set
            {
                UserCredentials.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        public string LastName
        {
            get => UserCredentials.LastName;
            set
            {
                UserCredentials.LastName = value;
                OnPropertyChanged("LastName");
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

        #region SignUpCommand

        private AsyncCommand _signUpCommand;

        public IAsyncCommand SignUpCommand => _signUpCommand ?? (_signUpCommand = new AsyncCommand(
                async (obj) =>
                {
                    string hashedPassword = PasswordEncoder.GetHash(Password);

                    AppNavHelper.ShowProgressBar();
                    bool emailExists = await Task.Run(() => UsersService.CheckIfUserExistsByEmail(Email));

                    if (emailExists)
                    {
                        ErrorMessage = "Email already registered";
                        AppNavHelper.HideProgressBar();
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
                        HashedPassword = PasswordEncoder.GetHash(Password),
                        Role = Role.Intern,
                        UserDetails = person,
                    };

                    bool userCreated = await Task.Run(() => UsersService.AddUserAsync(user));
                    if (userCreated)
                    {
                        AppNavHelper.NavigationService.Navigate(new AuthorizationView());
                        ErrorMessage = null;
                    }
                    else
                    {
                        ErrorMessage = "User was not created";
                    }
                    AppNavHelper.HideProgressBar();
                }, (obj) => IsValid));

        #endregion

        #region BackCommand

        private Command _backCommand;

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(
                (obj) =>
                {
                    AppNavHelper.NavigationService.Navigate(new AuthorizationView());
                    ErrorMessage = null;
                }));

        #endregion
    }
}
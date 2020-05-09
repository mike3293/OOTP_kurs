using System;
using System.Threading.Tasks;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Models;
using WpfClient.Services;

namespace WpfClient.ViewModels
{
    public class PendingUserViewModel : ViewModelBase
    {
        public PendingUser PendingUser;

        public Action RemoveDelegate;

        public PendingUserViewModel(PendingUser user, Action remove)
        {
            PendingUser = user;
            RemoveDelegate = remove;
        }

        public string Email
        {
            get => PendingUser.Email;
            set
            {
                PendingUser.Email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string FirstName
        {
            get => PendingUser.FirstName;
            set
            {
                PendingUser.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string LastName
        {
            get => PendingUser.LastName;
            set
            {
                PendingUser.LastName = value;
                OnPropertyChanged(nameof(LastName));
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string DisplayName => $"{FirstName} {LastName}";

        #region Validation
        private bool _userIsValid;
        public bool UserIsValid
        {
            get => _userIsValid;
            set
            {
                _userIsValid = value;
                OnPropertyChanged(nameof(UserIsValid));
            }
        }
        #endregion

        #region AddInternCommand

        private AsyncCommand _addInternCommand;

        public IAsyncCommand AddInternCommand => _addInternCommand ?? (_addInternCommand = new AsyncCommand(
                async (obj) =>
                {
                    User user = CreateDbUser();
                    AppNavHelper.ShowProgressBar();
                    bool userUpdated = await Task.Run(() => UsersService.UpdateUser(user));
                    AppNavHelper.HideProgressBar();

                    if (userUpdated)
                    {
                        RemoveDelegate();
                    }
                }));

        private User CreateDbUser(Role role = Role.Intern)
        {
            return new User()
            {
                Id = PendingUser.Id,
                Role = role,
                UserDetails = new Person()
                {
                    Id = PendingUser.DetailsId,
                    FirstName = PendingUser.FirstName,
                    LastName = PendingUser.LastName,
                }
            };
        }
        #endregion

        #region AddManagerCommand

        private AsyncCommand _addManagerCommand;

        public IAsyncCommand AddManagerCommand => _addManagerCommand ?? (_addManagerCommand = new AsyncCommand(
                async (obj) =>
                {
                    User user = CreateDbUser(Role.Manager);
                    AppNavHelper.ShowProgressBar();
                    bool userUpdated = await Task.Run(() => UsersService.UpdateUser(user));
                    AppNavHelper.HideProgressBar();

                    if (userUpdated)
                    {
                        RemoveDelegate();
                    }
                }));
        #endregion

        #region RemoveCommand

        private AsyncCommand _removeCommand;

        public IAsyncCommand RemoveCommand => _removeCommand ?? (_removeCommand = new AsyncCommand(
                async (obj) =>
                {
                    User user = CreateDbUser();
                    AppNavHelper.ShowProgressBar();
                    bool userDeleted = await Task.Run(() => UsersService.DeleteUser(user));
                    AppNavHelper.HideProgressBar();

                    if (userDeleted)
                    {
                        RemoveDelegate();
                    }
                }));
        #endregion
    }
}

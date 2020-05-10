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

        public Action OnUpdated;

        public Func<bool> CheckIfValid;

        public PendingUserViewModel(PendingUser user, Action update, Func<bool> checkIfValid)
        {
            PendingUser = user;
            OnUpdated = update;
            CheckIfValid = checkIfValid;
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

        #region AddInternCommand

        private AsyncCommandWithTimeout _addInternCommand;

        public IAsyncCommand AddInternCommand => _addInternCommand ?? (_addInternCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    User user = CreateDbUser();
                    AppNavHelper.ShowProgressBar();
                    bool userUpdated = await Task.Run(() => UsersService.UpdateUser(user));
                    Internship internship = new Internship()
                    {
                        Intern = user.UserDetails,
                        Manager = AppNavHelper.CurrentUser.UserDetails,
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddDays(30)
                    };
                    await Task.Run(() => InternshipsService.AddInternshipAsync(internship));
                    AppNavHelper.HideProgressBar();

                    if (userUpdated)
                    {
                        OnUpdated();
                    }
                },obj => CheckIfValid()));

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

        private AsyncCommandWithTimeout _addManagerCommand;

        public IAsyncCommand AddManagerCommand => _addManagerCommand ?? (_addManagerCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    User user = CreateDbUser(Role.Manager);
                    AppNavHelper.ShowProgressBar();
                    bool userUpdated = await Task.Run(() => UsersService.UpdateUser(user));
                    AppNavHelper.HideProgressBar();

                    if (userUpdated)
                    {
                        OnUpdated();
                    }
                }, obj => CheckIfValid()));
        #endregion

        #region RemoveCommand

        private AsyncCommandWithTimeout _removeCommand;

        public IAsyncCommand RemoveCommand => _removeCommand ?? (_removeCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    User user = CreateDbUser();
                    AppNavHelper.ShowProgressBar();
                    bool userDeleted = await Task.Run(() => UsersService.DeleteUser(user));
                    AppNavHelper.HideProgressBar();

                    if (userDeleted)
                    {
                        OnUpdated();
                    }
                }));
        #endregion
    }
}

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
        private AppNavHelper _appNavHelper = AppNavHelper.GetInstance();

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

        private async Task NotifyByEmail(Person manager)
        {
            await MailsService.SendEmailAsync(
                Email,
                "Manager approved your account",
                $"{manager.FirstName} {manager.LastName}",
                $"<h3>Your account approved.</h3>"
            );
        }

        private AsyncCommand _addInternCommand;

        public IAsyncCommand AddInternCommand => _addInternCommand ?? (_addInternCommand = new AsyncCommand(
                async (obj) =>
                {
                    User user = CreateDbUser();
                    _appNavHelper.IncrementTasksCounter();
                    bool userUpdated = await Task.Run(() => UsersService.UpdateUserAsync(user));
                    Person manager = _appNavHelper.CurrentUser.UserDetails;
                    Internship internship = new Internship()
                    {
                        Intern = user.UserDetails,
                        Manager = manager,
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddDays(30)
                    };
                    await Task.Run(() => InternshipsService.AddInternshipAsync(internship));

                    if (userUpdated)
                    {
                        await Task.Run(() => NotifyByEmail(manager));
                        OnUpdated();
                    }
                    _appNavHelper.DecrementTasksCounter();
                }, obj => CheckIfValid()));

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
                    _appNavHelper.IncrementTasksCounter();
                    bool userUpdated = await Task.Run(() => UsersService.UpdateUserAsync(user));
                    _appNavHelper.DecrementTasksCounter();

                    if (userUpdated)
                    {
                        await Task.Run(() => NotifyByEmail(_appNavHelper.CurrentUser.UserDetails));
                        OnUpdated();
                    }
                    _appNavHelper.DecrementTasksCounter();
                }, obj => CheckIfValid()));
        #endregion

        #region RemoveCommand

        private AsyncCommand _removeCommand;

        public IAsyncCommand RemoveCommand => _removeCommand ?? (_removeCommand = new AsyncCommand(
                async (obj) =>
                {
                    User user = CreateDbUser();
                    _appNavHelper.IncrementTasksCounter();
                    bool userDeleted = await Task.Run(() => UsersService.DeleteUserAsync(user));
                    _appNavHelper.DecrementTasksCounter();

                    if (userDeleted)
                    {
                        OnUpdated();
                    }
                }));
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ToastNotifications.Messages;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Helpers;
using WpfClient.Models;
using WpfClient.Services;
using WpfClient.Views;

namespace WpfClient.ViewModels
{
    public class ManagerViewModel : ViewModelBase
    {
        private AppNavHelper _appNavHelper = AppNavHelper.GetInstance();

        #region GetData
        private ObservableCollection<PendingUserViewModel> usersPendingApproval;

        private List<Internship> _internships;

        private string _searchQuery = "";

        private ObservableCollection<Internship> filteredInternships;

        private Internship selectedIntern;

        public ObservableCollection<PendingUserViewModel> UsersPendingApproval
        {
            get => usersPendingApproval;
            set
            {
                usersPendingApproval = value;
                OnPropertyChanged(nameof(UsersPendingApproval));
            }
        }

        public ObservableCollection<Internship> FilteredInternships
        {
            get => filteredInternships;
            set
            {
                filteredInternships = value;
                OnPropertyChanged(nameof(FilteredInternships));
            }
        }

        public Internship SelectedIntern
        {
            get => selectedIntern;
            set
            {
                selectedIntern = value;
                OnPropertyChanged(nameof(SelectedIntern));
            }
        }

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        public ManagerViewModel()
        {
            _ = GetInternships();
            _ = GetUsers();
        }

        private async Task GetInternships()
        {
            _appNavHelper.IncrementTasksCounter();
            List<Internship> internships = await Task.Run(() => InternshipsService.GetInternshipsByManagerIdAsync(_appNavHelper.CurrentUser.UserDetails.Id));
            _internships = internships.OrderBy(i => i.GetSearchData()).ToList();
            GetFilteredInternships();

            IEnumerable<Internship> outdatedInternships = _internships.Where(i => i.EndDate < DateTime.Today);
            if (outdatedInternships.Count() > 0)
            {
                foreach (Internship i in outdatedInternships)
                {
                    _appNavHelper.Notifier.ShowWarning($"You have outdeted internship:\nName: {i.Intern.FirstName} {i.Intern.LastName}\nEnd date: {i.EndDate.ToShortDateString()}");
                }
            }
            _appNavHelper.DecrementTasksCounter();
        }

        private async Task GetUsers()
        {
            _appNavHelper.IncrementTasksCounter();
            List<User> users = await Task.Run(() => UsersService.GetUsersWithoutRolesAsync());

            UsersPendingApproval = new ObservableCollection<PendingUserViewModel>(users.Select(u =>
            {
                PendingUser user = new PendingUser(u.Id, u.UserDetails.Id, u.Email, u.UserDetails.FirstName, u.UserDetails.LastName);
                return new PendingUserViewModel(
                    user,
                    () => { UsersPendingApproval.Remove(UsersPendingApproval.Where(us => us.PendingUser.Id == u.Id).Single()); _ = GetInternships(); },
                    () => IsValid
                    );
            }));
            _appNavHelper.DecrementTasksCounter();
        }

        private void GetFilteredInternships()
        {
            var filteredInternships = _internships.Where(i => i.GetSearchData().Contains(SearchQuery.ToLower()));
            FilteredInternships = new ObservableCollection<Internship>(filteredInternships);
        }
        #endregion

        #region ItemSelectedCommand
        private Command _itemSelectedCommand;

        public ICommand ItemSelectedCommand => _itemSelectedCommand ?? (_itemSelectedCommand = new Command(
                (obj) =>
                {
                    if (obj is Internship internship)
                    {
                        _appNavHelper.NavigationService.Navigate(new InternView(internship));
                    }
                }));

        #endregion

        #region TextChangedCommand

        private Command _textChangedCommand;

        public ICommand TextChangedCommand => _textChangedCommand ?? (_textChangedCommand = new Command(
                (obj) =>
                {
                    if (_internships != null)
                    {
                        GetFilteredInternships();
                    }
                }));

        #endregion
    }
}

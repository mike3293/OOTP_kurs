using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Models;
using WpfClient.Services;

namespace WpfClient.ViewModels
{
    public class ManagerViewModel : ViewModelBase
    {
        #region GetData
        private ObservableCollection<PendingUserViewModel> usersPendingApproval;

        public ObservableCollection<PendingUserViewModel> UsersPendingApproval
        {
            get => usersPendingApproval;
            set
            {
                usersPendingApproval = value;
                OnPropertyChanged(nameof(UsersPendingApproval));
            }
        }

        public ManagerViewModel() => GetUsers();

        private async Task GetUsers()
        {
            AppNavHelper.ShowProgressBar();
            var users = await Task.Run(() => UsersService.GetUsersWithoutRolesAsync());
            AppNavHelper.HideProgressBar();

            UsersPendingApproval = new ObservableCollection<PendingUserViewModel>(users.Select((u, i) =>
            {
                PendingUser user = new PendingUser(u.Id, u.UserDetails.Id, u.Email, u.UserDetails.FirstName, u.UserDetails.LastName);
                return new PendingUserViewModel(user, () => UsersPendingApproval.RemoveAt(i));
            }));
        }
        #endregion

        #region Validation
        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            set
            {
                _isValid = value;
                OnPropertyChanged(nameof(IsValid));
            }
        }
        #endregion
    }
}

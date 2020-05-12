using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClient.Commands;
using WpfClient.Services;
using WpfClient.Views;

namespace WpfClient.ViewModels
{
    public class NotUpprovedViewModel
    {
        #region BackCommand

        private Command _backCommand;

        public ICommand BackCommand => _backCommand ?? (_backCommand = new Command(
                (obj) =>
                {
                    AppNavHelper.GetInstance().NavigationService.Navigate(new AuthorizationView());
                }));

        #endregion
    }
}

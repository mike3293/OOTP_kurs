using WpfClient.Validations;
using WpfClient.ViewModels;

namespace WpfClient.Views
{
    public partial class AuthorizationView : PageWithValidation
    {
        public AuthorizationView() : base(new AuthorizationViewModel())
        {
            InitializeComponent();
        }
    }
}

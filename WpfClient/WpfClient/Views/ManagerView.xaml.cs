using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfClient.Services;
using WpfClient.Validations;
using WpfClient.ViewModels;

namespace WpfClient.Views
{
    public partial class ManagerView : PageWithValidation
    {
        public ManagerView() : base(new ManagerViewModel())
        {
            InitializeComponent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfClient.DataBase.Models;
using WpfClient.ViewModels;

namespace WpfClient.Views
{
    public partial class InternView : Page
    {
        public InternView(Internship internship)
        {
            InitializeComponent();

            DataContext = new InternViewModel(internship);
        }

        public InternView(User user)
        {
            InitializeComponent();

            DataContext = new InternViewModel(user);
        }
    }
}

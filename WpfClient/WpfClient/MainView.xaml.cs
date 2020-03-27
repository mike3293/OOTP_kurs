using System.Linq;
using System.Windows.Controls;
using WpfClient.BackEnd;

namespace WpfClient
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Page
    {
        public MainView()
        {
            InitializeComponent();


            using (BackEndContext db = new BackEndContext())
            {
                InternsList.ItemsSource = db.People.Where(p => !p.IsManager).ToList();
            }
        }

        private void InternsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
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

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            InternsList.SelectedItems.Clear();

            ListViewItem item = sender as ListViewItem;
            if (item != null)
            {
                item.IsSelected = true;
                InternsList.SelectedItem = item;
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListViewItem item = sender as ListViewItem;
            if (item != null && item.IsSelected)
            {
                MainWindow win2 = new MainWindow();
                win2.Show();
            }
        }
    }
}

using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using WpfClient.DataBase;
using WpfClient.Views;

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

            // TODO
            //using (DataBaseContext db = new DataBaseContext())
            //{
            //    InternsList.ItemsSource = db.People.ToList();
            //}
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
                MainWindowView win2 = new MainWindowView();
                win2.Show(); // TODO
            }
        }
    }
}

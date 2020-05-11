using System.Globalization;
using System.Windows;

namespace WpfClient
{
    public partial class App : Application
    {
        public App()
        {
            CultureInfo culture = new CultureInfo("en-US");
            culture.DateTimeFormat.FirstDayOfWeek = System.DayOfWeek.Monday;
            culture.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}

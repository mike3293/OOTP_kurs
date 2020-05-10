using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Services;
using WpfClient.Views;

namespace WpfClient.ViewModels
{
    public class InternViewModel : ViewModelBase
    {
        private Internship internship = new Internship() { Intern = new Person() };

        private readonly int _internId;

        public Internship Internship
        {
            get => internship;
            set
            {
                internship = value;
                OnPropertyChanged(nameof(Internship));
            }
        }

        #region GetData
        public InternViewModel(Internship internship)
        {
            _internId = internship.Intern.Id;
            _ = GetInternship(_internId);
        }

        public InternViewModel(User user)
        {
            _internId = user.UserDetails.Id;
            _ = GetInternship(_internId);
        }

        private async Task GetInternship(int id)
        {
            AppNavHelper.ShowProgressBar();
            Internship = await Task.Run(() => InternshipsService.GetInternshipByInternIdAsync(id));
            AppNavHelper.HideProgressBar();
        }
        #endregion

        public Visibility CanEdit => AppNavHelper.CurrentUser.Role == Role.Manager ? Visibility.Visible : Visibility.Hidden;

        #region UploadImageCommand

        private AsyncCommand _uploadImageCommand;

        public IAsyncCommand UploadImageCommand => _uploadImageCommand ?? (_uploadImageCommand = new AsyncCommand(
                async (obj) =>
                {
                    string fileName = GetFileName();
                    if (fileName == null)
                    {
                        return;
                    }

                    BitmapSource bSource = new BitmapImage(new Uri(fileName));
                    byte[] img = BitmapSourceToByteArray(bSource);

                    AppNavHelper.ShowProgressBar();
                    bool imgUpdated = await Task.Run(() => PeopleService.UpdatePersonImageAsync(Internship.Intern.Id, img));
                    AppNavHelper.HideProgressBar();

                    if (imgUpdated)
                    {
                        _ = GetInternship(_internId);
                        return;
                    }
                }));

        private string GetFileName()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "PNG Files (*.png)|*.png"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                return dlg.FileName;
            }
            return null;
        }

        private byte[] BitmapSourceToByteArray(BitmapSource image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
        #endregion

        #region GoBackCommand

        private Command _goBackCommand;

        public ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new Command(
                (obj) =>
                {
                    AppNavHelper.NavigationService.Navigate(new ManagerView());
                }));

        #endregion
    }
}
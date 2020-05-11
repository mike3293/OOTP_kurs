using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Models;
using WpfClient.Services;
using WpfClient.Views;

namespace WpfClient.ViewModels
{
    public class InternViewModel : ViewModelBase
    {
        private AppNavHelper _appNavHelper = AppNavHelper.GetInstance();

        private Internship internship = new Internship() { Intern = new Person() };

        private List<Assessment> assessments;

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

        public List<Assessment> Assessments
        {
            get => assessments;
            set
            {
                assessments = value;
                OnPropertyChanged(nameof(Assessments));
            }
        }

        #region GetData
        public InternViewModel(Internship internship)
        {
            _internId = internship.Intern.Id;
            _ = GetInternship(_internId);
            _ = GetAssessments(_internId);
        }

        public InternViewModel(User user)
        {
            _internId = user.UserDetails.Id;
            _ = GetInternship(_internId);
            _ = GetAssessments(_internId);
        }

        private async Task GetInternship(int id)
        {
            _appNavHelper.IncrementTasksCounter();
            Internship = await Task.Run(() => InternshipsService.GetInternshipByInternIdAsync(id));
            _appNavHelper.DecrementTasksCounter();
        }

        private async Task GetAssessments(int id)
        {
            _appNavHelper.IncrementTasksCounter();
            Assessments = await Task.Run(() => AssessmentsService.GetAssessmentsByInternIdAsync(id));
            _appNavHelper.DecrementTasksCounter();
        }
        #endregion

        public Visibility CanManage => _appNavHelper.CurrentUser.Role == Role.Manager ? Visibility.Visible : Visibility.Hidden;

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

                    _appNavHelper.IncrementTasksCounter();
                    bool imgUpdated = await Task.Run(() => PeopleService.UpdatePersonImageAsync(Internship.Intern.Id, img));
                    _appNavHelper.DecrementTasksCounter();

                    if (imgUpdated)
                    {
                        _ = GetInternship(_internId);
                        return;
                    }
                }));
        // TODO separate
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
                    _appNavHelper.NavigationService.Navigate(new ManagerView());
                }));

        #endregion

        #region InitAddingAssessmentCommand
        private bool _isAddingAssessment;

        public bool IsAddingAssessment
        {
            get => _isAddingAssessment;
            set
            {
                _isAddingAssessment = value;
                OnPropertyChanged(nameof(IsAddingAssessment));
            }
        }


        private Command _initAddingAssessmentCommand;

        public ICommand InitAddingAssessmentCommand => _initAddingAssessmentCommand ?? (_initAddingAssessmentCommand = new Command(
                (obj) =>
                {
                    IsAddingAssessment = true;
                }));

        #endregion

        #region CancelAssessmentAddingCommand

        private Command _cancelAssessmentAddingCommand;

        public ICommand CancelAssessmentAddingCommand => _cancelAssessmentAddingCommand ?? (_cancelAssessmentAddingCommand = new Command(
                (obj) =>
                {
                    IsAddingAssessment = false;
                }));

        #endregion

        #region AddAssessmentCommand

        private NewAssessment _newAssessment = new NewAssessment() { Date = DateTime.Today, Time = DateTime.Now };

        public NewAssessment NewAssessment
        {
            get => _newAssessment;
            set
            {
                _newAssessment = value;
                OnPropertyChanged(nameof(NewAssessment));
            }
        }

        private AsyncCommandWithTimeout _addAssessmentCommand;

        public IAsyncCommand AddAssessmentCommand => _addAssessmentCommand ?? (_addAssessmentCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    if (NewAssessment.Date == null)
                    {
                        NewAssessment.Date = DateTime.Today;
                    }
                    if (NewAssessment.Time == null)
                    {
                        NewAssessment.Time = DateTime.Now;
                    }

                    Assessment assessment = new Assessment()
                    {
                        Date = NewAssessment.Date.Date.AddHours(NewAssessment.Time.Hour).AddMinutes(NewAssessment.Time.Minute),
                        Location = NewAssessment.Location,
                        Topic = NewAssessment.Topic,
                        Internship = Internship,
                    };

                    _appNavHelper.IncrementTasksCounter();
                    bool assessmentAdded = await AssessmentsService.AddAssessmentAsync(assessment);
                    _appNavHelper.DecrementTasksCounter();

                    if (assessmentAdded)
                    {
                        IsAddingAssessment = false;
                    }
                }));

        #endregion
    }
}
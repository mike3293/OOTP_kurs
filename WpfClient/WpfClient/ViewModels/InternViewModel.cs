using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfClient.Commands;
using WpfClient.DataBase.Models;
using WpfClient.Helpers;
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

        public bool CanManage => _appNavHelper.CurrentUser.Role == Role.Manager;

        public bool CanFinish => Internship.IsCompleted != true && _appNavHelper.CurrentUser.Role == Role.Manager;

        #region UploadImageCommand

        private AsyncCommand _uploadImageCommand;

        public IAsyncCommand UploadImageCommand => _uploadImageCommand ?? (_uploadImageCommand = new AsyncCommand(
                async (obj) =>
                {
                    ImageEncodingHelper encoder = new ImageEncodingHelper();
                    string fileName = encoder.GetFileName();
                    if (fileName == null)
                    {
                        return;
                    }

                    byte[] img = encoder.PngImageToByteArray(fileName);

                    _appNavHelper.IncrementTasksCounter();
                    bool imgUpdated = await Task.Run(() => PeopleService.UpdatePersonImageAsync(Internship.Intern.Id, img));
                    _appNavHelper.DecrementTasksCounter();

                    if (imgUpdated)
                    {
                        _ = GetInternship(_internId);
                        return;
                    }
                }));
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
                    ClearAddingAssessment();
                }));

        #endregion

        #region AddAssessmentCommand

        private static Func<NewAssessment> CreateNewAssessment = () => new NewAssessment() { Date = DateTime.Today, Time = DateTime.Now };

        private NewAssessment _newAssessment = CreateNewAssessment();

        public NewAssessment NewAssessment
        {
            get => _newAssessment;
            set
            {
                _newAssessment = value;
                OnPropertyChanged(nameof(NewAssessment));
            }
        }

        private void ClearAddingAssessment()
        {
            NewAssessment = CreateNewAssessment();
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
                        ClearAddingAssessment();

                        _appNavHelper.IncrementTasksCounter();
                        User user = await UsersService.GetUserByPersonIdAsync(Internship.Intern.Id);
                        Person manager = _appNavHelper.CurrentUser.UserDetails;
                        await Task.Run(() => MailsService.SendEmailAsync(
                           user.Email,
                           $"You have new assessment \"{assessment.Topic}\"",
                           $"{manager.FirstName} {manager.LastName}",
                           $"<h3>Date: {assessment.Date}</h3><br><h4>Location: {assessment.Location}</h4>"
                       ));
                        _appNavHelper.DecrementTasksCounter();

                        _ = GetAssessments(_internId);
                    }
                }));

        #endregion

        #region DeleteAssessmentCommand

        private AsyncCommandWithTimeout _deleteAssessmentCommand;

        public IAsyncCommand DeleteAssessmentCommand => _deleteAssessmentCommand ?? (_deleteAssessmentCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    if (obj is Assessment assessment)
                    {
                        _appNavHelper.IncrementTasksCounter();
                        bool assessmentDeleted = await AssessmentsService.DeleteAssessmentAsync(assessment.Id);
                        _appNavHelper.DecrementTasksCounter();

                        if (assessmentDeleted)
                        {
                            _ = GetAssessments(_internId);
                        }
                    }
                }));

        #endregion

        #region EndInternshipCommand
        private AsyncCommandWithTimeout _endInternshipCommand;

        public IAsyncCommand EndInternshipCommand => _endInternshipCommand ?? (_endInternshipCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    _appNavHelper.IncrementTasksCounter();
                    bool internshipUpdated = await Task.Run(() => InternshipsService.CompleteInternshipAsync(Internship.Id));

                    if (internshipUpdated)
                    {
                        User user = await UsersService.GetUserByPersonIdAsync(Internship.Intern.Id);
                        Person manager = _appNavHelper.CurrentUser.UserDetails;
                        await Task.Run(() => MailsService.SendEmailAsync(
                            user.Email,
                            "The internship was completed",
                            $"{manager.FirstName} {manager.LastName}",
                            $"<h3>Duration: {(DateTime.Today - Internship.StartDate).TotalDays} days</h3>"
                        ));
                        _appNavHelper.NavigationService.Navigate(new ManagerView());
                    }
                    _appNavHelper.DecrementTasksCounter();
                }));
        #endregion

        #region EditEndDateCommand

        public bool _isEditingEndDate;

        public bool IsEditingEndDate
        {
            get => _isAddingAssessment;
            set
            {
                _isAddingAssessment = value;
                OnPropertyChanged(nameof(IsEditingEndDate));
                OnPropertyChanged(nameof(IsNotEditingEndDate));
            }
        }

        public bool IsNotEditingEndDate => !IsEditingEndDate;


        private Command _editEndDateCommand;

        public ICommand EditEndDateCommand => _editEndDateCommand ?? (_editEndDateCommand = new Command(
                (obj) =>
                {
                    IsEditingEndDate = true;
                }));

        #endregion

        #region SaveEndDateCommand

        private AsyncCommandWithTimeout _saveEndDateCommand;

        public IAsyncCommand SaveEndDateCommand => _saveEndDateCommand ?? (_saveEndDateCommand = new AsyncCommandWithTimeout(
                async (obj) =>
                {
                    _appNavHelper.IncrementTasksCounter();
                    bool internshipUpdated = await Task.Run(() => InternshipsService.UpdateInternshipEndDateAsync(Internship));
                    _appNavHelper.DecrementTasksCounter();

                    if (internshipUpdated)
                    {
                        IsEditingEndDate = false;
                        _ = GetInternship(_internId);
                    }
                }));

        #endregion
    }
}
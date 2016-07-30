using MEB.EasyTimeLog.Model;
using MEB.EasyTimeLog.Model.Exception;
using MEB.EasyTimeLog.Domain;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.ViewModel.Property;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    public class NewLogViewModel : NewLogViewModelProperty, IViewModel
    {
        private readonly IRepository<LogEntity, Guid> _logRepository;
        private readonly IRepository<TaskEntity, Guid> _taskRepository;

        private bool _isPropertiesValid;

        public NewLogView View { get; }
        
        public NewLogViewModel(IRepository<LogEntity, Guid> logRepository, IRepository<TaskEntity, Guid> taskRepository)
        {
            // Create a new instance of the log view.
            View = new NewLogView
            {
                DataContext = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            // Get a new instance of the domain repository.
            _logRepository = logRepository;
            _taskRepository = taskRepository;

            // Create a new instance of the commands.
            LogCommand = new DelegateCommand(DoLogCommand, CanLogCommand);

            // This is used to control, if the button is enabled.
            _isPropertiesValid = false;
            PropertyChanged += (s, e) =>
            {
                ValidateProperties();
            };
        }

        public void Load()
        {
            // Load all task names into the properties.
            _taskRepository.GetAll().ToList().ForEach(task => Tasks.Add(task.Name));

            // Start and show the view.
            View.Show();
        }

        private void ValidateProperties()
        {
            TimeSpan from;
            TimeSpan to;

            // Try to parse the properties.
            _isPropertiesValid =
                TimeSpan.TryParseExact(TimeFrom, TimeUtil.TimeSpanFormat, null, out from) &&
                TimeSpan.TryParseExact(TimeTo, TimeUtil.TimeSpanFormat, null, out to) &&
                !string.IsNullOrEmpty(SelectedTask) &&
                TimeSpan.Compare(from, to) == -1;

            // Raise the can execute on the log button.
            LogCommand.RaiseCanExecuteChanged();
        }

        #region Commands
        public DelegateCommand LogCommand { get; set; }

        private void DoLogCommand(object sender)
        {
            // Create a task object.
            var task = _taskRepository.Save(new TaskEntity()
            {
                Name = SelectedTask
            });
            

            var from = TimeSpan.ParseExact(TimeFrom, TimeUtil.TimeSpanFormat, null);
            var to = TimeSpan.ParseExact(TimeTo, TimeUtil.TimeSpanFormat, null);

            // Try to create a time entry.
            try
            {
                _logRepository.Save(new LogEntity
                {
                    Task = task.Id,
                    TaskRef = task,
                    TimeFrom = from,
                    TimeTo = to,
                    Day = SelectedDate,
                    Notes = Notes
                });

                // Close window.
                View.Close();
            }
            catch (TimeConflictException timeConflict)
            {
                // Show a nice message for the user...
                MessageBox.Show(
                    "The time is conflict with an existing entry you made.\n" +
                    $"The existing entry is {_taskRepository.Get(timeConflict.ConflictingEntry.Task).Name}: {timeConflict.ConflictingEntry.TimeFrom.ToString(TimeUtil.TimeSpanFormat)} - {timeConflict.ConflictingEntry.TimeTo.ToString(TimeUtil.TimeSpanFormat)}.\n" +
                    $"Yours is {task.Name}: {from.ToString(TimeUtil.TimeSpanFormat)} - {to.ToString(TimeUtil.TimeSpanFormat)}.",
                    "Time Conflict Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.OK);
                
            }
            catch (Exception ex)
            {
                // Print the exception for debugging.
                Debug.WriteLine(ex);

                // Show a some how nice message...
                MessageBox.Show(
                    "There was an unexpected error.",
                    "Unknown Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error,
                    MessageBoxResult.OK);
            }

        }

        private bool CanLogCommand(object arg)
        {
            return _isPropertiesValid;
        }
        #endregion
    }
}

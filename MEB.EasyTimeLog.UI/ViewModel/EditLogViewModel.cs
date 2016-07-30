using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using MEB.EasyTimeLog.Domain;
using MEB.EasyTimeLog.Model;
using MEB.EasyTimeLog.Model.Exception;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.ViewModel.Property;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    public class EditLogViewModel : EditLogViewModelProperty, IViewModel
    {
        private readonly IRepository<LogEntity, Guid> _logRepository;
        private readonly IRepository<TaskEntity, Guid> _taskRepository;
        private readonly LogEntity _entity;

        private bool _isPropertiesValid;

        public EditLogView View { get; }

        public EditLogViewModel(IRepository<LogEntity, Guid>  logRepository, IRepository<TaskEntity, Guid> taskRepository,LogEntity entity)
        {
            // Set the entity that is going to be changed.
            _logRepository = logRepository;
            _taskRepository = taskRepository;
            _entity = entity;

            // Create a new instance of the log view.
            View = new EditLogView
            {
                DataContext = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            // Create a new instance of the commands.
            EditCommand = new DelegateCommand(DoEditCommand, CanEditCommand);
            DeleteCommand = new DelegateCommand(DoDeleteCommand, CanDeleteCommand);

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
            _taskRepository.GetAll().ToList().ForEach(task => TaskNames.Add(task.Name));

            TaskName = _entity.TaskRef.Name;
            SelectedDate = _entity.Day;
            TimeFrom = _entity.TimeFrom.ToString(TimeUtil.TimeSpanFormat);
            TimeTo = _entity.TimeTo.ToString(TimeUtil.TimeSpanFormat);
            Notes = _entity.Notes;

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
                !string.IsNullOrEmpty(TaskName) &&
                TimeSpan.Compare(from, to) == -1;

            // Raise the can execute on the log button.
            EditCommand.RaiseCanExecuteChanged();
        }

        #region Commands
        public DelegateCommand EditCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        private void DoEditCommand(object sender)
        {
            // Create a task object.
            var task = _taskRepository.Save(new TaskEntity
            {
                Name = TaskName
            });

            var from = TimeSpan.ParseExact(TimeFrom, TimeUtil.TimeSpanFormat, null);
            var to = TimeSpan.ParseExact(TimeTo, TimeUtil.TimeSpanFormat, null);

            // Try to create a time entry.
            try
            {
                var tempEntity = new LogEntity(_entity.Id)
                {
                    Task = task.Id,
                    TaskRef = task,
                    Day = SelectedDate,
                    TimeFrom = from,
                    TimeTo = to,
                    Notes = Notes
                };

                // Save the temp object.
                _logRepository.Save(tempEntity);

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

        private bool CanEditCommand(object arg)
        {
            return _isPropertiesValid;
        }

        private void DoDeleteCommand(object sender)
        {
            var messageResult = MessageBox.Show(
                "Are you sure, there is no turning back?",
                "Delete log",
                MessageBoxButton.YesNo,
                MessageBoxImage.Exclamation);

            // If the user is not saying yes, then just return.
            if (messageResult != MessageBoxResult.Yes)
            {
                return;
            }

            // Try to create a time entry.
            try
            {
                // Delete the log entity object.
                _logRepository.Delete(_entity.Id);

                // Close window.
                View.Close();
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

            // Close window.
            View.Close();
        }

        private bool CanDeleteCommand(object arg)
        {
            return _isPropertiesValid;
        }
        #endregion
    }
}
using MEB.EasyTimeLog.Model;
using MEB.EasyTimeLog.Model.Exception;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.ViewModel.Property;
using System;
using System.Diagnostics;
using System.Windows;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    class LogViewModel : LogViewModelProperty, IViewModel
    {
        private LogView _view;
        private DomainRepository _repo;

        private bool _isPropertiesValid;

        public LogView View { get { return _view; } }

        public LogViewModel()
        {
            // Create a new instance of the log view.
            _view = new LogView
            {
                DataContext = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            // Get a new instance of the domain repository.
            _repo = DomainFactory.GetRepository();

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
            _repo.GetAllTasks().ForEach(task => Tasks.Add(task.Name));

            // Start and show the view.
            _view.Show();
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
            var task = _repo.CreateTaskEntry(SelectedTask);
            var from = TimeSpan.ParseExact(TimeFrom, TimeUtil.TimeSpanFormat, null);
            var to = TimeSpan.ParseExact(TimeTo, TimeUtil.TimeSpanFormat, null);

            // Try to create a time entry.
            try
            {
                // The time spans are validated, so they are just parsed.
                _repo.CreateTimeEntry(
                    task, 
                    from, 
                    to, 
                    SelectedDate);

                // Close window.
                _view.Close();
            }
            catch (TimeConflictException timeConflict)
            {
                // Show a nice message for the user...
                MessageBox.Show(
                    "The time is conflict with an existing entry you made.\n" +
                    $"The existing entry is {timeConflict.ConflictingEntry.Task.Name}: {timeConflict.ConflictingEntry.TimeFrom.ToString(TimeUtil.TimeSpanFormat)} - {timeConflict.ConflictingEntry.TimeTo.ToString(TimeUtil.TimeSpanFormat)}.\n" +
                    $"Yours is {task.Name}: {from.ToString(TimeUtil.TimeSpanFormat)} - {to.ToString(TimeUtil.TimeSpanFormat)}.",
                    "Time Conflict Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation,
                    MessageBoxResult.OK);
            }
            catch(Exception ex)
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

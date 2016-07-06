using MEB.EasyTimeLog.Model;
using MEB.EasyTimeLog.Model.Exception;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.ViewModel.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            // Create a new instance of the domain repository.
            _repo = new DomainRepository();

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
                TimeSpan.TryParse(TimeFrom, out from) &&
                TimeSpan.TryParse(TimeTo, out to) && 
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

            // Try to create a time entry.
            try
            {
                // The time spans are validated, so they are just parsed.
                _repo.CreateTimeEntry(
                    task, 
                    TimeSpan.Parse(TimeFrom), 
                    TimeSpan.Parse(TimeTo), 
                    SelectedDate);
            }
            catch (TimeConflictException timeConflict)
            {

            }
        }

        private bool CanLogCommand(object arg)
        {
            return _isPropertiesValid;
        }
        #endregion
    }
}

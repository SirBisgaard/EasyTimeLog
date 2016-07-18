using MEB.EasyTimeLog.UI.ViewModel.Property;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using MEB.EasyTimeLog.Domain;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    public class MainViewModel : MainViewModelProperty, IViewModel
    {
        private readonly IRepository<LogEntity, Guid> _logRepository;
        private readonly IRepository<TaskEntity, Guid> _taskRepository;

        public MainView View { get; }

        public MainViewModel()
        {
            // Create a new instance of the main view.
            View = new MainView
            {
                DataContext = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
            };

            // Get a new instance of the domain repository.
            _logRepository = RepositoryFactory.GetRepository<LogEntity, Guid>();
            _taskRepository = RepositoryFactory.GetRepository<TaskEntity, Guid>();

            // Create a new instance of the commands.
            LogCommand = new DelegateCommand(DoLogCommand, CanLogCommand);
            EditCommand = new DelegateCommand(DoEditCommand, CanEditCommand);

            // Raise can execute when properties change.
            PropertyChanged += (s, e) =>
            {
                LogCommand.RaiseCanExecuteChanged();

                // If the selected sort type is changed, reload the sort list.
                if (e.PropertyName == nameof(SelectedSortType))
                {
                    LoadSortList();
                }

                // If the selected sort type is changed, reload the sort list.
                if (e.PropertyName == nameof(SelectedSortValue))
                {
                    LoadLogs();
                }
            };
        }

        public void Load()
        {
            // Refresh the sort values.
            LoadSortList();
            // Refresh the logs.
            LoadLogs();

            // Start and show the view.
            View.Show();
        }

        private void LoadSortList()
        {
            // Clear the list if its dirty.
            if (SortValues.Count > 0)
            {
                SortValues.Clear();
            }

            var listOfSortValues = new List<string>();

            if (SelectedSortType == "Tasks")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(_taskRepository.GetAll().Select(task => task.ToString()));
            }
            if (SelectedSortType == "Day")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleDays(_logRepository.GetAll()));
            }
            if (SelectedSortType == "Week")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleWeeks(_logRepository.GetAll()));
            }
            if (SelectedSortType == "Month")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleMonths(_logRepository.GetAll()));
            }
            if (SelectedSortType == "Year")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleYear(_logRepository.GetAll()));
            }

            // Load all logs into the properties.
            listOfSortValues.ForEach(s => SortValues.Add(s));

            // Select the first sort value from the list.
            if (SortValues.Count > 0)
            {
                // Select first value.
                SelectedSortValue = SortValues[0];
            }
        }

        public void LoadLogs()
        {
            // Clear the list if its dirty.
            if (LogList.Count > 0)
            {
                LogList.Clear();
            }

            List<LogEntity> entries = null;

            if (SelectedSortType == "Tasks")
            {
                // Load all logs into the properties.
                entries = _logRepository.GetAll().Where(e => _taskRepository.Get(e.Task)?.Name == SelectedSortValue).ToList();
            }
            if (SelectedSortType == "Day")
            {
                DateTime selectedDay;

                if (DateTime.TryParse(SelectedSortValue, out selectedDay))
                {
                    // Load all logs into the properties.
                    entries = _logRepository.GetAll().Where(entry => entry.Day == selectedDay).ToList();
                }
            }
            if (SelectedSortType == "Week")
            {
                // Load all logs into the properties.
                entries = _logRepository.GetAll().Where(e => $"{e.Day.Year} - Week {TimeUtil.Calendar.GetWeekOfYear(e.Day, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}" == SelectedSortValue).ToList();
            }
            if (SelectedSortType == "Month")
            {
                // Load all logs into the properties.
                entries = _logRepository.GetAll().Where(e => e.Day.ToString(TimeUtil.DateMonthFormat) == SelectedSortValue).ToList();
            }
            if (SelectedSortType == "Year")
            {
                // Load all logs into the properties.
                entries = _logRepository.GetAll().Where(e => e.Day.Year.ToString() == SelectedSortValue).ToList();
            }

            if (entries == null)
            {
                TotalHours = "0";
                return;
            }

            foreach (var entry in entries)
            {
                // ReSharper disable once UseStringInterpolation
                LogList.Add(string.Format(
                    "{3} {0}: {1} - {2} Duration={4}",
                    _taskRepository.Get(entry.Task),
                    entry.TimeFrom.ToString(TimeUtil.TimeSpanFormat),
                    entry.TimeTo.ToString(TimeUtil.TimeSpanFormat),
                    entry.Day.ToShortDateString(),
                    TimeUtil.GetDuration(entry.TimeFrom, entry.TimeTo)));
            }

            TotalHours = TimeUtil.GetDuration(entries);
        }

        #region Commands
        public DelegateCommand LogCommand { get; }
        public DelegateCommand EditCommand { get; }

        private void DoLogCommand(object sender)
        {
            // Disable interface.
            CanExecute = false;

            // Create a instance of the log view model.
            var viewModel = new LogViewModel();
            // Set the ownership and a listener on the close event.
            viewModel.View.Owner = View;
            viewModel.View.Closed += (s, e) =>
            {
                // Refresh the sort values.
                LoadSortList();
                // Refresh the logs.
                LoadLogs();

                // Enable the view and focus it.
                CanExecute = true;
                View.Focus();
            };

            // Load the view model.
            viewModel.Load();
        }

        private bool CanLogCommand(object arg)
        {
            return CanExecute;
        }

        private void DoEditCommand(object sender)
        {
            throw new NotImplementedException();
        }

        private bool CanEditCommand(object arg)
        {
            return false;
        }
        #endregion
    }
}

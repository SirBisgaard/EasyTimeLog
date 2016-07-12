using MEB.EasyTimeLog.UI.ViewModel.Property;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using MEB.EasyTimeLog.Model.Domain;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    public class MainViewModel : MainViewModelProperty, IViewModel
    {
        private MainView _view;
        private DomainRepository _repo;

        public MainView View { get { return _view; } }

        public MainViewModel()
        {
            // Create a new instance of the main view.
            _view = new MainView
            {
                DataContext = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
            };

            // Get a new instance of the domain repository.
            _repo = DomainFactory.GetRepository();

            // Create a new instance of the commands.
            LogCommand = new DelegateCommand(DoLogCommand, CanLogCommand);

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
            _view.Show();
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
                _repo.GetAllTasks().ForEach(e => listOfSortValues.Add(e.Name));
            }
            if (SelectedSortType == "Day")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleDays(_repo.GetAllLogs()));
            }
            if (SelectedSortType == "Week")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleWeeks(_repo.GetAllLogs()));
            }
            if (SelectedSortType == "Month")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleMonths(_repo.GetAllLogs()));
            }
            if (SelectedSortType == "Year")
            {
                // Get the list of sort values.
                listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleYear(_repo.GetAllLogs()));
            }

            // Load all logs into the properties.
            listOfSortValues.ForEach(value => SortValues.Add(value));

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

            IEnumerable<TimeEntity> entries = null;

            if (SelectedSortType == "Tasks")
            {
                // Load all logs into the properties.
                entries = _repo.GetAllLogs().Where(e => e.Task.Name == SelectedSortValue);
                foreach (var entry in entries)
                {
                    LogList.Add(entry.ToString(TimeUtil.DefaultTimeEntryFormat));
                }
            }
            if (SelectedSortType == "Day")
            {
                DateTime selectedDay;

                if (DateTime.TryParse(SelectedSortValue, out selectedDay))
                {
                    // Load all logs into the properties.
                    entries = _repo.GetAllLogs().Where(entry => entry.Day == selectedDay);
                    foreach (var entry in entries)
                    {
                        LogList.Add(entry.ToString(TimeUtil.DefaultTimeEntryFormat));
                    }
                }
            }
            if (SelectedSortType == "Week")
            {
                // Load all logs into the properties.
                entries = _repo.GetAllLogs().Where(e => $"{e.Day.Year} - Week {TimeUtil.Calendar.GetWeekOfYear(e.Day, CalendarWeekRule.FirstDay, DayOfWeek.Monday)}" == SelectedSortValue);
                foreach (var entry in entries)
                {
                    LogList.Add(entry.ToString(TimeUtil.DefaultTimeEntryFormat));
                }
            }
            if (SelectedSortType == "Month")
            {
                // Load all logs into the properties.
                entries = _repo.GetAllLogs().Where(e => e.Day.ToString(TimeUtil.DateMonthFormat) == SelectedSortValue);
                foreach (var entry in entries)
                {
                    LogList.Add(entry.ToString(TimeUtil.DefaultTimeEntryFormat));
                }
            }
            if (SelectedSortType == "Year")
            {
                // Load all logs into the properties.
                entries = _repo.GetAllLogs().Where(e => e.Day.Year.ToString() == SelectedSortValue);
                foreach (var entry in entries)
                {
                    LogList.Add(entry.ToString(TimeUtil.DefaultTimeEntryFormat));
                }
            }

            TotalHours = TimeUtil.GetDuration(entries);
        }

        #region Commands
        public DelegateCommand LogCommand { get; set; }

        private void DoLogCommand(object sender)
        {
            // Disable interface.
            CanExecute = false;

            // Create a instance of the log view model.
            var viewModel = new LogViewModel();
            // Set the ownership and a listener on the close event.
            viewModel.View.Owner = _view;
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
        #endregion
    }
}

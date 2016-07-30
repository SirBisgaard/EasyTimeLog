using MEB.EasyTimeLog.UI.ViewModel.Property;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Navigation;
using MEB.EasyTimeLog.DataAccess;
using MEB.EasyTimeLog.Domain;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    public class MainViewModel : MainViewModelProperty, IViewModel
    {
        private readonly IRepository<LogEntity, Guid> _logRepository;
        private readonly IRepository<TaskEntity, Guid> _taskRepository;
        private readonly ISettingsStore<string, string> _settingsStore;

        public MainView View { get; }

        public MainViewModel(ISettingsStore<string, string> settingsStore, IRepository<LogEntity, Guid> logRepository, IRepository<TaskEntity, Guid> taskRepository)
        {
            // Create a new instance of the main view.
            View = new MainView
            {
                DataContext = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
            };

            // Get a new instance of the domain repository.
            _logRepository = logRepository;
            _taskRepository = taskRepository;

            // Get a instance of the settings store.
            _settingsStore = settingsStore;

            // Create a new instance of the commands.
            LogCommand = new DelegateCommand(DoLogCommand, CanLogCommand);
            EditCommand = new DelegateCommand(DoEditCommand, CanEditCommand);

            // Raise can execute when properties change.
            PropertyChanged += (s, e) =>
            {
                LogCommand.RaiseCanExecuteChanged();

                if (e.PropertyName == nameof(SelectedSortType))
                {
                    // Load the sort values.
                    LoadSortValues();
                }
                if (e.PropertyName == nameof(SelectedSortValue))
                {
                    // Load the log entities.
                    LoadLogEntities();
                }

                // If the selected value is changed, notify the edit button.
                if (e.PropertyName == nameof(SelectedValue))
                {
                    EditCommand.RaiseCanExecuteChanged();
                }
            };

            View.Closed += (s, e) =>
            {
                _settingsStore.Save(nameof(SelectedSortType), SelectedSortType);
                _settingsStore.Save(nameof(SelectedSortValue), SelectedSortValue);
            };
        }

        public void Load()
        {
            // Refresh the sort values.
            LoadSortValues();

            // Load the last used settinggs.
            SelectedSortType = _settingsStore.Get(nameof(SelectedSortType));
            SelectedSortValue = _settingsStore.Get(nameof(SelectedSortValue));

            // Select the default if empty.
            if (string.IsNullOrEmpty(SelectedSortValue))
            {
                SelectedSortType = "Task";
            }

            // Start and show the view.
            View.Show();
        }

        private void LoadSortValues()
        {
            var selectedSortValue = SelectedSortValue;

            // Clear the lists before adding new data.
            SortValues.Clear();
            LogList.Clear();

            // Get the sort types.
            var listOfSortValues = new List<string>();
            switch (SelectedSortType)
            {
                case "Task":
                    // Get the list of sort values.
                    listOfSortValues.AddRange(_taskRepository.GetAll().Select(task => task.ToString()));
                    break;
                case "Day":
                    // Get the list of sort values.
                    listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleDays(_logRepository.GetAll()));
                    break;
                case "Week":
                    // Get the list of sort values.
                    listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleWeeks(_logRepository.GetAll()));
                    break;
                case "Month":
                    // Get the list of sort values.
                    listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleMonths(_logRepository.GetAll()));
                    break;
                case "Year":
                    // Get the list of sort values.
                    listOfSortValues.AddRange(TimeUtil.GetListOfAvalibleYear(_logRepository.GetAll()));
                    break;
            }

            // Insert the sort values into the list.
            listOfSortValues.ForEach(s => SortValues.Add(s));

            // Select the last selected value if it is still in the list,
            // or just select the first item in list.
            SelectedSortValue = SortValues.Contains(selectedSortValue) ?
                selectedSortValue : SortValues.FirstOrDefault();
        }

        private void LoadLogEntities()
        {
            // Clear the list of logs.
            LogList.Clear();


            // Get all the logs from the selected sort type and sort value.
            var entities = _logRepository.GetAll(SelectedSortType, SelectedSortValue);
            
            // Add the entities to the list.
            foreach (var entry in entities)
            {
                LogList.Add(entry);
            }

            // Calculate the total hours.
            TotalHours = LogList.Count == 0 ? "0" : TimeUtil.GetDuration(entities);
        }


        #region Commands
        public DelegateCommand LogCommand { get; }
        public DelegateCommand EditCommand { get; }

        private void DoLogCommand(object sender)
        {
            // Disable interface.
            CanExecute = false;

            // Create a instance of the log view model.
            var viewModel = new NewLogViewModel(_logRepository, _taskRepository);
            // Set the ownership and a listener on the close event.
            viewModel.View.Owner = View;
            viewModel.View.Closed += (s, e) =>
            {
                // Refresh the sort values.
                LoadSortValues();

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
            // Disable interface.
            CanExecute = false;

            // Create a instance of the log view model.
            var viewModel = new EditLogViewModel(_logRepository, _taskRepository, SelectedValue);
            // Set the ownership and a listener on the close event.
            viewModel.View.Owner = View;
            viewModel.View.Closed += (s, e) =>
            {
                // Refresh the sort values.
                LoadSortValues();

                // Enable the view and focus it.
                CanExecute = true;
                View.Focus();
            };

            // Load the view model.
            viewModel.Load();
        }

        private bool CanEditCommand(object arg)
        {
            return SelectedValue != null;
        }
        #endregion
    }
}

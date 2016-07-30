using MEB.EasyTimeLog.UI.Common;
using System.Collections.ObjectModel;
using MEB.EasyTimeLog.Domain;

namespace MEB.EasyTimeLog.UI.ViewModel.Property
{
    public class MainViewModelProperty : Observable
    {
        private bool _canExecute;

        private ObservableCollection<LogEntity> _logList;
        private ObservableCollection<string> _sortTypes;
        private ObservableCollection<string> _sortValues;

        private string _selectedSortValue;
        private string _selectedSortType;
        private string _totalHours;
        private LogEntity _selectedValue;

        public MainViewModelProperty()
        {
            // Set can execute to true, so the interface is enabled.
            _canExecute = true;

            // Set default values.
            _logList = new ObservableCollection<LogEntity>();
            _sortTypes = new ObservableCollection<string>
            {
                "Task",
                "Day",
                "Week",
                "Month",
                "Year"
            };
            _sortValues = new ObservableCollection<string>();
            _totalHours = "0";
            _selectedSortValue = string.Empty;
        }

        public bool CanExecute
        {
            set { SetField(ref _canExecute, value); }
            get { return _canExecute; }
        }

        public ObservableCollection<LogEntity> LogList
        {
            set { SetField(ref _logList, value); }
            get { return _logList; }
        }

        public ObservableCollection<string> SortTypes
        {
            set { SetField(ref _sortTypes, value); }
            get { return _sortTypes; }
        }

        public ObservableCollection<string> SortValues
        {
            set { SetField(ref _sortValues, value); }
            get { return _sortValues; }
        }

        public string SelectedSortValue
        {
            set { SetField(ref _selectedSortValue,value); }
            get { return _selectedSortValue; }
        }

        public string SelectedSortType
        {
            set { SetField(ref _selectedSortType, value); }
            get { return _selectedSortType; }
        }

        public LogEntity SelectedValue
        {
            set { SetField(ref _selectedValue, value); }
            get { return _selectedValue; }
        }

        public string TotalHours
        {
            set { SetField(ref _totalHours, value); }
            get { return _totalHours; }
        }
    }
}

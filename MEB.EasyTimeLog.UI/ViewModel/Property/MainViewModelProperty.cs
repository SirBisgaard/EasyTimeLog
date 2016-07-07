using MEB.EasyTimeLog.UI.Common;
using System.Collections.ObjectModel;

namespace MEB.EasyTimeLog.UI.ViewModel.Property
{
    public class MainViewModelProperty : Observable
    {
        private bool _canExecute;

        private ObservableCollection<string> _typeList;
        private ObservableCollection<string> _logList;
        private ObservableCollection<string> _sortValues;

        private string _selectedSortValue;
        private string _selectedSortType;
        private string _totalHours;

        public MainViewModelProperty()
        {
            // Set can execute to true, so the interface is enabled.
            _canExecute = true;

            // Set default values.
            _typeList = new ObservableCollection<string>();
            _logList = new ObservableCollection<string>();
            _sortValues = new ObservableCollection<string>();
            _selectedSortType = "Tasks";
            _totalHours = "0";
            _selectedSortValue = string.Empty;
        }

        public bool CanExecute
        {
            set { SetField(ref _canExecute, value); }
            get { return _canExecute; }
        }

        public ObservableCollection<string> LogList
        {
            set { SetField(ref _logList, value); }
            get { return _logList; }
        }

        public ObservableCollection<string> SortValues
        {
            set { SetField(ref _sortValues, value); }
            get { return _sortValues; }
        }
        
        public string SelectedSortValue
        {
            set { SetField(ref _selectedSortValue, value?.Replace("System.Windows.Controls.ComboBoxItem: ", "")); }
            get { return _selectedSortValue; }
        }

        public string SelectedSortType
        {
            set { SetField(ref _selectedSortType, value?.Replace("System.Windows.Controls.ComboBoxItem: ", "")); }
            get { return _selectedSortType; }
        }

        public string TotalHours
        {
            set { SetField(ref _totalHours, value); }
            get { return _totalHours; }
        }
    }
}

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

        private string _selectedType;
        private string _selectedSortValue;
        private string _selectedSortType;

        public MainViewModelProperty()
        {
            // Set can execute to true, so the interface is enabled.
            _canExecute = true;

            // Set default values.
            _typeList = new ObservableCollection<string>();
            _logList = new ObservableCollection<string>();
            _sortValues = new ObservableCollection<string>();
            _selectedType = "Tasks";
            _selectedSortType = "Day";
            _selectedSortValue = string.Empty;
        }

        public bool CanExecute
        {
            set { SetField(ref _canExecute, value); }
            get { return _canExecute; }
        }
        
        public ObservableCollection<string> TypeList
        {
            set { SetField(ref _typeList, value); }
            get { return _typeList; }
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

        public string SelectedType
        {
            set { SetField(ref _selectedType, value); }
            get { return _selectedType; }
        }

        public string SelectedSortValue
        {
            set { SetField(ref _selectedSortValue, value); }
            get { return _selectedSortValue; }
        }

        public string SelectedSortType
        {
            set { SetField(ref _selectedSortType, value); }
            get { return _selectedSortType; }
        }
    }
}

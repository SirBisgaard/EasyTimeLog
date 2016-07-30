using System;
using System.Collections.ObjectModel;
using MEB.EasyTimeLog.UI.Common;

namespace MEB.EasyTimeLog.UI.ViewModel.Property
{
    public class EditLogViewModelProperty : Observable
    {
        private bool _canExecute;

        private ObservableCollection<string> _taskNames;
        private string _taskName;

        private DateTime _selectedDate;
        private string _timeFrom;
        private string _timeTo;

        public EditLogViewModelProperty()
        {
            CanExecute = true;

            // Set default values.
            _taskNames = new ObservableCollection<string>();
            _selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            _timeFrom = string.Empty;
            _timeTo = string.Empty;

            _taskName = string.Empty;
        }

        public bool CanExecute
        {
            set { SetField(ref _canExecute, value); }
            get { return _canExecute; }
        }

        public ObservableCollection<string> TaskNames
        {
            set { SetField(ref _taskNames, value); }
            get { return _taskNames; }
        }

        public string TaskName
        {
            set { SetField(ref _taskName, value); }
            get { return _taskName; }
        }

        public DateTime SelectedDate
        {
            set { SetField(ref _selectedDate, value); }
            get { return _selectedDate; }
        }

        public string TimeFrom
        {
            set { SetField(ref _timeFrom, value); }
            get { return _timeFrom; }
        }

        public string TimeTo
        {
            set { SetField(ref _timeTo, value); }
            get { return _timeTo; }
        }
    }
}
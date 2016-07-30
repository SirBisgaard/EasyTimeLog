using System;
using System.Collections.ObjectModel;
using MEB.EasyTimeLog.UI.Common;

namespace MEB.EasyTimeLog.UI.ViewModel.Property
{
    public class NewLogViewModelProperty : Observable
    {
        private bool _canExecute;

        private ObservableCollection<string> _tasks;

        private string _selectedTask;
        private DateTime _selectedDate;
        private string _timeFrom;
        private string _timeTo;

        public NewLogViewModelProperty()
        {
            // Set can execute to true, so the interface is enabled.
            _canExecute = true;

            // Set default values.
            _tasks = new ObservableCollection<string>();
            _selectedTask = string.Empty;
            _selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            _timeFrom = string.Empty;
            _timeTo = string.Empty;
        }

        public bool CanExecute
        {
            set { SetField(ref _canExecute, value); }
            get { return _canExecute; }
        }

        public ObservableCollection<string> Tasks
        {
            set { SetField(ref _tasks, value); }
            get { return _tasks; }
        }

        public string SelectedTask
        {
            set { SetField(ref _selectedTask, value); }
            get { return _selectedTask; }
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

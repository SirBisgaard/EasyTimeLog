using MEB.EasyTimeLog.UI.Common;
using System;

namespace MEB.EasyTimeLog.UI.ViewModel.Property
{
    public class LogViewModelProperty : Observable
    {
        private bool _canExecute;
        private DateTime _selectedDate;
        private string _timeFrom;
        private string _timeTo;

        public LogViewModelProperty()
        {
            // Set can execute to true, so the interface is enabled.
            _canExecute = true;
            _selectedDate = DateTime.Now;
            _timeFrom = string.Empty;
            _timeTo = string.Empty;
        }

        public bool CanExecute
        {
            set { SetField(ref _canExecute, value); }
            get { return _canExecute; }
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

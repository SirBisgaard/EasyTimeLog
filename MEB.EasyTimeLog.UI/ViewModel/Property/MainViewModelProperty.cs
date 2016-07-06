using MEB.EasyTimeLog.UI.Common;

namespace MEB.EasyTimeLog.UI.ViewModel.Property
{
    public class MainViewModelProperty : Observable
    {
        private bool _canExecute;

        public MainViewModelProperty()
        {
            // Set can execute to true, so the interface is enabled.
            _canExecute = true;
        }

        public bool CanExecute
        {
            set { SetField(ref _canExecute, value); }
            get { return _canExecute; }
        }
    }
}

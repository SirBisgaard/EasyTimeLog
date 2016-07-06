using MEB.EasyTimeLog.UI.ViewModel.Property;
using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.Common;
using MEB.EasyTimeLog.Model;

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

            // Create a new instance of the domain repository.
            _repo = new DomainRepository();

            // Create a new instance of the commands.
            LogCommand = new DelegateCommand(DoLogCommand, CanLogCommand);

            // Raise can execute when properties change.
            PropertyChanged += (s, e) =>
            {
                LogCommand.RaiseCanExecuteChanged();
            };
        }

        public void Load()
        {
            // Start and show the view.
            _view.Show();
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

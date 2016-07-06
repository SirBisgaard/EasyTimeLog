using MEB.EasyTimeLog.UI.ViewModel.Property;
using MEB.EasyTimeLog.UI.View;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    public class MainViewModel : MainViewModelProperty, IViewModel
    {
        private MainView _view;

        public MainViewModel()
        {
            // Create a new instance of the main view.
            _view = new MainView
            {
                DataContext = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen
            };
        }

        public void Load()
        {
            // Start and show the view.
            _view.Show();
        }
    }
}

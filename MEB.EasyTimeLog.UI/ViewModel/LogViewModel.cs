using MEB.EasyTimeLog.UI.View;
using MEB.EasyTimeLog.UI.ViewModel.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEB.EasyTimeLog.UI.ViewModel
{
    class LogViewModel : LogViewModelProperty, IViewModel
    {
        private LogView _view;

        public LogView View { get { return _view; } }

        public LogViewModel()
        {
            // Create a new instance of the log view.
            _view = new LogView
            {
                DataContext = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
            };
        }

        public void Load()
        {
            // Start and show the view.
            _view.Show();
        }
    }
}

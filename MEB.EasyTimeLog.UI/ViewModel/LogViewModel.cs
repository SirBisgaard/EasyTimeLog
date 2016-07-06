using MEB.EasyTimeLog.UI.Common;
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
        private bool _isPropertiesValid;

        public LogView View { get { return _view; } }

        public LogViewModel()
        {
            // Create a new instance of the log view.
            _view = new LogView
            {
                DataContext = this,
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner
            };

            // Create a new instance of the commands.
            LogCommand = new DelegateCommand(DoLogCommand, CanLogCommand);

            // This is used to control, if the button is enabled.
            _isPropertiesValid = false;
            PropertyChanged += (s, e) =>
            {
                ValidateProperties();
            };
        }

        public void Load()
        {
            // Start and show the view.
            _view.Show();
        }

        private void ValidateProperties()
        {
            TimeSpan from;
            TimeSpan to;

            // Try to parse from and to, and save the result.
            _isPropertiesValid = TimeSpan.TryParse(TimeFrom, out from) && TimeSpan.TryParse(TimeTo, out to);

            // Raise the can execute on the log button.
            LogCommand.RaiseCanExecuteChanged();
        }

        #region Commands
        public DelegateCommand LogCommand { get; set; }

        private void DoLogCommand(object sender)
        {

        }

        private bool CanLogCommand(object arg)
        {
            return _isPropertiesValid;
        }
        #endregion
    }
}

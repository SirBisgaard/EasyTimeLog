using MEB.EasyTimeLog.UI.ViewModel;
using System.Windows;

namespace MEB.EasyTimeLog.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var viewModel = new MainViewModel();
            viewModel.Load();
        }
    }
}

using MEB.EasyTimeLog.UI.ViewModel;
using System.Windows;
using MEB.EasyTimeLog.DataAccess;
using MEB.EasyTimeLog.Model;

namespace MEB.EasyTimeLog.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var dataStore = new JsonDataStore();
            var taskRepo = new TaskRepository(dataStore);

            var viewModel = new MainViewModel(new SettingsStore(dataStore), new LogRepository(dataStore, taskRepo), taskRepo);
            viewModel.Load();
        }
    }
}

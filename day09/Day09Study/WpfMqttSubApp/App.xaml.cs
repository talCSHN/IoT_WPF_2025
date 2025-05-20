using MahApps.Metro.Controls.Dialogs;
using System.Configuration;
using System.Data;
using System.Windows;
using WpfSmartHomeApp.ViewModels;
using WpfSmartHomeApp.Views;

namespace WpfSmartHomeApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var coordinator = DialogCoordinator.Instance; // new DialogCoordinator()와 동일
            var viewModel = new MainViewModel(coordinator);
            var view = new MainView
            {
                DataContext = viewModel,
            };
            view.ShowDialog();
        }
    }

}

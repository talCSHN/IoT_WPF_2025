using BusanRestaurantApp.ViewModels;
using BusanRestaurantApp.Views;
using MahApps.Metro.Controls.Dialogs;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BusanRestaurantApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            IDialogCoordinator coordinator = DialogCoordinator.Instance;
            var viewModel = new BusanMatjibViewModel(coordinator);
            var view = new BusanMatjibView
            {
                DataContext = viewModel,
            };
            view.ShowDialog();
        }
    }

}

using System.Configuration;
using System.Data;
using System.Windows;
using WpfBookRentalShop01.ViewModels;
using WpfBookRentalShop01.Views;

namespace WpfBookRentalShop01
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var viewModel = new MainViewModel();
            var view = new MainView
            {
                DataContext = viewModel
            };
            //var view = new MainView();
            //view.DataContext = viewModel;
            view.ShowDialog();
        }
    }

}

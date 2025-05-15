using _2025_MovieFinder.Helpers;
using _2025_MovieFinder.ViewModels;
using _2025_MovieFinder.Views;
using MahApps.Metro.Controls.Dialogs;
using System.Configuration;
using System.Data;
using System.Windows;

namespace _2025_MovieFinder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Common.DIALOGCOORDINATOR = DialogCoordinator.Instance;
            var viewModel = new MoviesViewModel(Common.DIALOGCOORDINATOR);
            var view = new MoviesView
            {
                DataContext = viewModel
            };
            view.Show();
        }
    }

}

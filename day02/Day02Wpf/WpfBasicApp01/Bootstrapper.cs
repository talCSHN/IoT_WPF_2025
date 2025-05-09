using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfBasicApp01.ViewModels;

namespace WpfBasicApp01
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();
            _container.Singleton<WindowManager, WindowManager>();
            _container.Singleton<IDialogCoordinator, IDialogCoordinator>();
            _container.PerRequest<MainViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            //base.OnStartup(sender, e);
            // App.xaml의 StartupUri와 동일한 역할
            // MainViewModel과 동일한 이름의 View 찾아서 바인딩 후 실행
            DisplayRootViewForAsync<MainViewModel>();
        }
    }
}

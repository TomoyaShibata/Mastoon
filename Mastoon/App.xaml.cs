using Mastoon.Views;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using System.Windows;

namespace Mastoon
{
    public partial class App : Application
    {
        private IUnityContainer Container { get; } = new UnityContainer();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory(x => this.Container.Resolve(x));
            this.Container.Resolve<MainWindow>().Show();
        }
    }
}

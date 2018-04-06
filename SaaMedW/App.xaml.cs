using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SaaMedW.Service;

namespace SaaMedW
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var logonservice = (ILogonService)ServiceLocator.Instance.GetService(typeof(ILogonService));
            var loginViewModel = new LoginViewModel();
            var f = new frmLogin { DataContext = loginViewModel };
            var showDialog = f.ShowDialog();
            if (showDialog != null && !showDialog.Value)
            {
                Current.Shutdown();
                return;
            }
            if (logonservice.RegisterUser(loginViewModel.Server,
                    loginViewModel.Database, loginViewModel.Login, loginViewModel.Password))
            {
                var storage = (ILocalStorage)ServiceLocator.Instance.GetService(typeof(ILocalStorage));
                storage.SetServerName(Environment.UserName, loginViewModel.Server);
                storage.SetDatabaseName(Environment.UserName, loginViewModel.Database);
                storage.SetLoginName(Environment.UserName, loginViewModel.Login);
                this.ShutdownMode = ShutdownMode.OnMainWindowClose;
                var modelview = new MasterWindowViewModel();
                var window = new MasterWindow();
                window.DataContext = modelview;
                Current.MainWindow = window;
                window.Show();
                ActivateView(new ExecTypes() { View = typeof(ReceiveView), ViewModel = typeof(ReceiveViewModel) });
            }
            else
            {
                Current.Shutdown();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public static void ActivateView(object o)
        {
            var ob = o as ExecTypes;
            var window = (MasterWindow)Current.MainWindow;

            // создаем экземпляр View
            var f = (UserControl)Activator.CreateInstance(ob.View);
            // создаем экземпляр ViewModel
            var vm = Activator.CreateInstance(ob.ViewModel);
            f.DataContext = vm;
            window.Wplace.Children.Clear();
            window.Wplace.Children.Add(f);
        }
    }
}

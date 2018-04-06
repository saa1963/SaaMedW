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
using SaaMedW.ViewModel;
using System.IO;

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

            var logpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "saamedw", "logs");
            if (!Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }
            log4net.GlobalContext.Properties["LogFileName"] = logpath + "\\log.txt";
            log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger("MAINLOGGER");

            using (var ctx = new SaaMedEntities())
            {
                var serviceUser = ctx.Users.Where(s => s.Login == "Service").FirstOrDefault();
                if (serviceUser.Password == null)
                {
                    var hash = new System.Security.Cryptography.SHA1CryptoServiceProvider()
                        .ComputeHash(System.Text.Encoding.ASCII.GetBytes("rfktdfkf"));
                    serviceUser.Password = hash;
                    ctx.SaveChanges();
                }
            }

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
            if (logonservice.RegisterUser(loginViewModel.Login, loginViewModel.Password))
            {
                var storage = (ILocalStorage)ServiceLocator.Instance.GetService(typeof(ILocalStorage));
                storage.SetLoginName(Environment.UserName, loginViewModel.Login);
                this.ShutdownMode = ShutdownMode.OnMainWindowClose;
                var modelview = new MasterWindowViewModel();
                var window = new MasterWindow();
                window.DataContext = modelview;
                Current.MainWindow = window;
                window.Show();
                //ActivateView(new ExecTypes() { View = typeof(ReceiveView), ViewModel = typeof(ReceiveViewModel) });

                log.Info("Старт приложения");
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

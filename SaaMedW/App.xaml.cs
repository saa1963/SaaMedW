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
using System.Reflection;
using System.Windows.Media;

namespace SaaMedW
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MessageBox.Show("111");
            base.OnStartup(e);

            log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger(typeof(App));
            log.Info("Начало работы");

            try
            {
                using (var ctx = new SaaMedEntities())
                {
                    var serviceUser = ctx.Users.Where(s => s.Login == "Service").FirstOrDefault();
                    if (serviceUser == null)
                    {
                        serviceUser = new Users
                        {
                            Disabled = false,
                            Fio = "Service",
                            Login = "Service",
                            Role = 0
                        };
                        ctx.Users.Add(serviceUser);
                        ctx.SaveChanges();
                    }
                    serviceUser = ctx.Users.Where(s => s.Login == "Service").FirstOrDefault();
                    if (serviceUser.Password == null)
                    {
                        var hash = new System.Security.Cryptography.SHA1CryptoServiceProvider()
                            .ComputeHash(System.Text.Encoding.ASCII.GetBytes("rfktdfkf"));
                        serviceUser.Password = hash;
                        ctx.SaveChanges();
                    }
                }
            }
            catch(Exception e1)
            {
                log.Error("Ошибка регистрации сервисного пользователя", e1);
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
                var window = new MasterWindow
                {
                    DataContext = modelview
                };
                Current.MainWindow = window;
                window.Show();

                log.Info("Старт приложения");
            }
            else
            {
                var mess = $"Пользователь {loginViewModel.Login} не прошел регистрацию.";
                log.Error(mess);
                MessageBox.Show(mess);
                Current.Shutdown();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        public static void ActivateView(object o)
        {
            if (o is ExecTypes)
            {
                var ob = o as ExecTypes;
                var window = (MasterWindow)Current.MainWindow;

                // создаем экземпляр View
                var f = (UserControl)Activator.CreateInstance(ob.View);
                // создаем экземпляр ViewModel
                var vm = Activator.CreateInstance(ob.ViewModel);
                f.DataContext = vm;
                PropertyInfo prop = vm.GetType().GetProperty("Form", BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    prop.SetValue(vm, f, null);
                }
                window.Wplace.Children.Clear();
                if (f.Tag != null)
                {
                    var tb = new TextBlock
                    {
                        Text = f.Tag.ToString(),
                        Padding = new Thickness(10),
                        Background = Brushes.AliceBlue,
                        FontSize = 20,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    DockPanel.SetDock(tb, Dock.Top);
                    window.Wplace.Children.Add(tb);
                }
                window.Wplace.Children.Add(f);
            }
            else
            {
                MessageBox.Show("Функция не реализована.");
            }
        }
    }
}

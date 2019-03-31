using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SaaMedW.Service;

namespace SaaMedW
{
    public class LoginViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        private string m_password;
        log4net.ILog log;

        public ObservableCollection<VmUsers> UsersList { get; set; }
            = new ObservableCollection<VmUsers>();
        public VmUsers CurrentUser { get; set; }

        public LoginViewModel()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
            using (var ctx = new SaaMedEntities())
            {
                foreach (var o in ctx.Users)
                {
                    UsersList.Add(new VmUsers(o));
                }
            }
            Login = Options.GetParameter<string>(enumParameterType.Последний_логин);
        }

        public string Login
        {
            get { return CurrentUser.Login; }
            set
            {
                CurrentUser = UsersList.SingleOrDefault(s => s.Login == value);
                OnPropertyChanged("Login");
                OnPropertyChanged("CurrentUser");
            }
        }

        public string Password
        {
            get { return m_password; }
            set
            {
                string savedvalue = m_password;
                if (value != savedvalue)
                {
                    m_password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        //public RelayCommand CmdAccept { get; set; }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Login")
                {
                    if (String.IsNullOrWhiteSpace(Login))
                        result = "Не заполнено поле 'Пользователь'";
                }
                return result;
            }
        }

        public string Error
        {
            get { return null; }
        }
    }
}

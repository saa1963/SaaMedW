﻿using System;
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
        private readonly ILocalStorage storage;
        private string m_login;
        private string m_password;
        log4net.ILog log;

        public ObservableCollection<VmUsers> UsersList { get; set; }

        public LoginViewModel()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
            storage = (ILocalStorage)ServiceLocator.Instance.GetService(typeof(ILocalStorage));
            m_login = storage.GetLoginName(Environment.UserName);
            //foreach(var o in )
        }

        public string Login
        {
            get { return m_login; }
            set
            {
                string savedvalue = m_login;
                if (value != savedvalue)
                {
                    m_login = value;
                    OnPropertyChanged("Login");
                }
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

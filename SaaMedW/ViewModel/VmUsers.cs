using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmUsers : INotifyPropertyChanged
    {
        private Users m_users;
        public event PropertyChangedEventHandler PropertyChanged;
        public VmUsers()
        {
            m_users = new Users();
        }
        public VmUsers(Users users)
        {
            m_users = users;
        }
        public string Fio
        {
            get => m_users.Fio;
            set
            {
                m_users.Fio = value;
                OnPropertyChanged("Fio");
            }
        }
        public string Login
        {
            get => m_users.Login;
            set
            {
                m_users.Login = value;
                OnPropertyChanged("Login");
            }
        }
        public byte[] Password
        {
            get => m_users.Password;
            set
            {
                m_users.Password = value;
                OnPropertyChanged("Password");
            }
        }
        public int Role
        {
            get => m_users.Role;
            set
            {
                m_users.Role = value;
                OnPropertyChanged("Role");
            }
        }
        public bool Disabled
        {
            get => m_users.Disabled;
            set
            {
                m_users.Disabled = value;
                OnPropertyChanged("Disabled");
            }
        }
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

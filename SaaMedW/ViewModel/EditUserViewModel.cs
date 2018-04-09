using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW.ViewModel
{
    public class EditUserViewModel : ViewModelBase, IDataErrorInfo
    {
        private string m_fio;
        private string m_login;
        private string m_password;
        private readonly ObservableCollection<Role> m_role = 
            new ObservableCollection<Role> {
                new Role { Id = 0, Name = "Администратор" },
                new Role { Id = 1, Name = "Пользователь"}
            };
        private bool m_disabled;

        public EditUserViewModel()
        {

        }

        public string Fio
        {
            get => m_fio;
            set
            {
                if (value != m_fio)
                {
                    m_fio = value;
                    OnPropertyChanged("Fio");
                }
            }
        }

        public string Login
        {
            get => m_login;
            set
            {
                if (value != m_login)
                {
                    m_login = value;
                    OnPropertyChanged("Login");
                }
            }
        }

        public string Password
        {
            get => m_password;
            set
            {
                if (value != m_password)
                {
                    m_password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public ObservableCollection<Role> Roles
        {
            get { return m_role; }
        }

        public Role RoleSel
        {
            get { return viewroles.CurrentItem as Role; }
            set { viewroles.MoveCurrentTo(value); }
        }

        private ICollectionView viewroles
        {
            get
            {
                return CollectionViewSource.GetDefaultView(Roles);
            }
        }

        public bool Disabled
        {
            get => m_disabled;
            set
            {
                if (value != m_disabled)
                {
                    m_disabled = value;
                    OnPropertyChanged("Disabled");
                }
            }
        }

        public bool IsEnablePassword { get; set; } = true;

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "Fio")
                {
                    if (String.IsNullOrWhiteSpace(Fio))
                        result = "Не заполнено поле 'ФИО'";
                }
                if (columnName == "Login")
                {
                    if (String.IsNullOrWhiteSpace(Login))
                        result = "Не заполнено поле 'Пользователь'";
                }
                return result;
            }
        }

        public string Error => "";
    }
}

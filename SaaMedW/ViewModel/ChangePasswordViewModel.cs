using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    class ChangePasswordViewModel : ViewModelBase, IDataErrorInfo
    {
        private string m_newpassword;
        private string m_reppassword;
        public string NewPassword
        {
            get => m_newpassword;
            set
            {
                m_newpassword = value;
                OnPropertyChanged("NewPassword");
            }
        }
        public string RepPassword
        {
            get => m_reppassword;
            set
            {
                m_reppassword = value;
                OnPropertyChanged("RepPassword");
            }
        }
        public string this[string columnName]
        {
            get
            {
                string result = String.Empty;
                if (columnName == "NewPassword")
                {
                    if (String.IsNullOrWhiteSpace(NewPassword))
                        result = "Не заполнено поле 'Новый пароль'";
                }
                if (columnName == "RepPassword")
                {
                    if (String.IsNullOrWhiteSpace(RepPassword))
                        result = "Не заполнено поле 'Повторить'";
                }
                return result;
            }
        }

        public string Error => "";
    }
}

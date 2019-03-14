using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    class ChangePasswordViewModel : NotifyPropertyChanged
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
    }
}

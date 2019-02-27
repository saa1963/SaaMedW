using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaaMedW
{
    class MultiEditViewModel: NotifyPropertyChanged
    {
        private string m_Text;
        public string Text
        {
            get => m_Text;
            set
            {
                m_Text = value;
                OnPropertyChanged("Text");
            }
        }
    }
}

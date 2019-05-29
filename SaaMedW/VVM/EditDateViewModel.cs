using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class EditDateViewModel: NotifyPropertyChanged
    {
        private DateTime m_Dt;
        private string m_Header = "Наименование";

        public EditDateViewModel() { }

        public DateTime Dt
        {
            get => m_Dt;
            set
            {
                m_Dt = value;
                OnPropertyChanged("Dt");
            }
        }
        public string Header
        {
            get => m_Header;
            set
            {
                m_Header = value;
                OnPropertyChanged("Header");
            }
        }
    }
}


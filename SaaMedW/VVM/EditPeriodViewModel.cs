using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class EditPeriodViewModel: NotifyPropertyChanged
    {
        private DateTime m_Dt1;
        public DateTime Dt1
        {
            get => m_Dt1;
            set
            {
                m_Dt1 = value;
                OnPropertyChanged("Dt1");
            }
        }
        private DateTime m_Dt2;
        public DateTime Dt2
        {
            get => m_Dt2;
            set
            {
                m_Dt2 = value;
                OnPropertyChanged("Dt2");
            }
        }
    }
}

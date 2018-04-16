using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class GraphicViewModel : ViewModelBase
    {
        private DateTime m_dt1;
        private DateTime m_dt2;
        private const int m_DaysInWeek = 7;
        private const int m_WeeksInMonth = 6;
        private ObservableCollection

        public GraphicViewModel(DateTime dt)
        {
            var dt1 = new DateTime(dt.Year, dt.Month, 1);
            while (dt1.DayOfWeek != DayOfWeek.Monday)
            {
                dt1 = dt1.AddDays(-1);
            }
            Dt1 = dt1;
            var dt2 = Dt1.AddMonths(1).AddDays(-1);
            while (dt1.DayOfWeek != DayOfWeek.Sunday)
            {
                dt2 = dt2.AddDays(1);
            }
            Dt2 = dt2;
        }
        public DateTime Dt1
        {
            get => m_dt1;
            set
            {
                m_dt1 = value;
                OnPropertyChanged("Dt1");
            }
        }
        public DateTime Dt2
        {
            get => m_dt2;
            set
            {
                m_dt2 = value;
                OnPropertyChanged("Dt2");
            }
        }
    }
}

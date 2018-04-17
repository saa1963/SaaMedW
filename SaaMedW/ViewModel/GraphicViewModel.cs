using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class GraphicViewModel : ViewModelBase
    {
        SaaMedEntities ctx = new SaaMedEntities();
        private DateTime m_dt1;
        private DateTime m_dt2;
        private const int m_DaysInWeek = 7;
        private const int m_WeeksInMonth = 6;
        private VmGraphic[] m_mas = new VmGraphic[42];

        public GraphicViewModel():this(DateTime.Today) { }

        public GraphicViewModel(DateTime dt)
        {
            var dt1 = new DateTime(dt.Year, dt.Month, 1);
            while (dt1.DayOfWeek != DayOfWeek.Monday)
            {
                dt1 = dt1.AddDays(-1);
            }
            Dt1 = dt1;
            Dt2 = Dt1.AddDays(m_DaysInWeek * m_WeeksInMonth - 1);
            DateTime d = Dt1;
            int i = 0;
            while (d <= Dt2)
            {
                m_mas[i] = VmGraphic.GetGraphic(ctx, d, null);
                d = d.AddDays(1);
                i++;
            }
        }
        public VmGraphic[] Mas
        {
            get => m_mas;
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class DateIntervalsViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();

        private DateTime m_Dt;
        public DateTime Dt
        {
            get => m_Dt;
            set
            {
                m_Dt = value;
                OnPropertyChanged("Dt");
            }
        }
        private int m_PersonalId;
        public int PersonalId
        {
            get => m_PersonalId;
            set
            {
                m_PersonalId = value;
                OnPropertyChanged("PersonalId");
            }
        }
        public ObservableCollection<TimeInterval> Intervals { get; set; }
            = new ObservableCollection<TimeInterval>();

        public void Fill()
        {
            var intervals = ctx.Graphic
                .Where(s => s.Dt.Date == Dt.Date && s.PersonalId == PersonalId)
                .Select(s => new TimeInterval(s.Dt, s.H1, s.M1, s.H2, s.M2))
                .OrderBy(s => s.Begin);
            foreach(var o in intervals)
            {
                Intervals.Add(o);
            }
        }
    }
}

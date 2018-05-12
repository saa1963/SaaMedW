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
        public ObservableCollection<VisitTimeInterval> Intervals { get; set; }
            = new ObservableCollection<VisitTimeInterval>();

        public void Fill()
        {
            var intervals = ctx.Graphic
                .Where(s => s.Dt.Year == Dt.Year && s.Dt.Month == Dt.Month
                    && s.Dt.Day == Dt.Day && s.PersonalId == PersonalId)
                    .OrderBy(s => s.Dt).ThenBy(s => s.H1).ThenBy(s => s.M1);
            foreach(var o in intervals)
            {
                var ob = new VisitTimeInterval(o.Dt, o.H1, o.M1, o.H2, o.M2);
                ob.Dt = Dt;
                ob.PersonalId = PersonalId;
                Intervals.Add(ob);
            }
        }
    }
}

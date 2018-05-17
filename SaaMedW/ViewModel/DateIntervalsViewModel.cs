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

        public PersonalVisitsViewModel Parent { get; set; }
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
        public Benefit Benefit { get; set; }
        public ObservableCollection<VisitTimeInterval> Intervals { get; set; }
            = new ObservableCollection<VisitTimeInterval>();

        public int Fill()
        {
            var t_intervals = new List<VisitTimeInterval>();
            var intervalsFromGraphic = ctx.Graphic
                .Where(s => s.Dt.Year == Dt.Year && s.Dt.Month == Dt.Month
                    && s.Dt.Day == Dt.Day && s.PersonalId == PersonalId)
                    .OrderBy(s => s.Dt).ThenBy(s => s.H1).ThenBy(s => s.M1);
            var intervalsFromVisit = ctx.Visit
                .Where(s => s.Dt.Year == Dt.Year && s.Dt.Month == Dt.Month 
                    && s.Dt.Day == Dt.Day && s.PersonalId == PersonalId)
                    .OrderBy(s => s.Dt);
            foreach (var o in intervalsFromVisit)
            {
                var ob = new TimeInterval()
                { Begin = o.Dt, Interval = new TimeSpan(0, o.Duration, 0) };
                    t_intervals.Add(new VisitTimeInterval(ob)
                    { PersonalId = this.PersonalId, Dt = this.Dt, typeTimeInterval = TypeTimeInterval.Visit, Parent = this, VisitId = o.Id });
            }
            foreach (var o in intervalsFromGraphic)
            {
                var ob = new TimeInterval(o.Dt, o.H1, o.M1, o.H2, o.M2);
                var obs = ob.Split(Benefit.Duration);
                foreach(var obs0 in obs)
                {
                    var vti = new VisitTimeInterval(obs0)
                    { Dt = this.Dt, PersonalId = this.PersonalId, typeTimeInterval = TypeTimeInterval.Graphic, Parent = this };
                    if (t_intervals.All(s => !s.IsIntersected(vti)))
                        t_intervals.Add(vti);
                }
            }
            foreach(var o in t_intervals.Where(s => s.Begin > DateTime.Now).OrderBy(s => s))
            {
                Intervals.Add(o);
            }
            return Intervals.Count();
        }
    }
}

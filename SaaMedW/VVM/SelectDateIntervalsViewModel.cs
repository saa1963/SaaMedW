using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SaaMedW
{
    public class SelectDateIntervalsViewModel: ViewModelBase
    {
        public SelectPersonalVisitsViewModel Parent { get; set; }
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

        public ObservableCollection<SelectVisitTimeInterval> Intervals { get; set; }
            = new ObservableCollection<SelectVisitTimeInterval>();

        public int Fill()
        {
            using (var ctx = new SaaMedEntities())
            {
                int personalId = Parent.PersonalId;
                int duration = Parent.Parent.Duration;
                var t_intervals = new List<SelectVisitTimeInterval>();
                var intervalsFromGraphic = ctx.Graphic
                    .Where(s => s.Dt.Year == Dt.Year && s.Dt.Month == Dt.Month
                        && s.Dt.Day == Dt.Day && s.PersonalId == personalId)
                        .OrderBy(s => s.Dt).ThenBy(s => s.H1).ThenBy(s => s.M1);
                var intervalsFromVisit = ctx.Visit
                    .Where(s => s.Dt.Year == Dt.Year && s.Dt.Month == Dt.Month
                        && s.Dt.Day == Dt.Day && s.PersonalId == personalId)
                        .OrderBy(s => s.Dt);
                foreach (var o in intervalsFromVisit)
                {
                    var ob = new TimeInterval()
                    { Begin = o.Dt, Interval = new TimeSpan(0, o.Duration, 0) };
                    t_intervals.Add(new SelectVisitTimeInterval(ob)
                    { TypeTimeInterv = TypeTimeInterval.Visit, Parent = this });
                }
                foreach (var o in intervalsFromGraphic)
                {
                    var ob = new TimeInterval(o.Dt, o.H1, o.M1, o.H2, o.M2);
                    var obs = ob.Split(duration);
                    foreach (var obs0 in obs)
                    {
                        var vti = new SelectVisitTimeInterval(obs0)
                        { TypeTimeInterv = TypeTimeInterval.Graphic, Parent = this };
                        if (t_intervals.All(s => !s.IsIntersected(vti)))
                            t_intervals.Add(vti);
                    }
                }
                foreach (var o in t_intervals.Where(s => s.Begin > DateTime.Now).OrderBy(s => s))
                {
                    Intervals.Add(o);
                }
                return Intervals.Count();
            }
        }
    }
}
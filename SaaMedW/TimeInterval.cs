using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class TimeInterval : INotifyPropertyChanged, IComparable<TimeInterval>
    {
        private DateTime m_begin;
        private TimeSpan m_interval;

        public DateTime Begin
        {
            get => m_begin;
            set
            {
                m_begin = value;
                OnPropertyChanged("Begin");
                OnPropertyChanged("DisplayName");
            }
        }
        public TimeSpan Interval
        {
            get => m_interval;
            set
            {
                m_interval = value;
                OnPropertyChanged("Interval");
                OnPropertyChanged("DisplayName");
            }
        }
        public DateTime End
        {
            get => Begin.AddTicks(Interval.Ticks);
        }

        public TimeInterval() { }
        public TimeInterval(TimeInterval ti)
        {
            Begin = ti.Begin;
            Interval = ti.Interval;
        }
        public TimeInterval(DateTime bg, int h1, int m1, int h2, int m2): 
            this(bg, h1, m1, (h2 * 60 + m2) - (h1 * 60 + m1))
        {
        }
        public TimeInterval(DateTime bg, int h1, int m1, int m2)
        {
            Begin = bg.Date.AddMinutes(h1 * 60 + m1);
            Interval = new TimeSpan(0, m2, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
        public string DisplayName
        {
            get => this.ToString();
        }
        public override string ToString()
        {
            return Begin.Hour.ToString("0") + ":" + Begin.Minute.ToString("00") + "-" + 
                End.Hour.ToString("0") + ":" + End.Minute.ToString("00");
        }
        public bool IsIntersected(TimeInterval i)
        {
            if (i.End <= this.Begin || this.End <= i.Begin)
            {
                return false;
            }
            else
                return true;
        }
        public ObservableCollection<TimeInterval> Split(int duration)
        {
            TimeSpan ts = new TimeSpan(0, duration, 0);
            var rt = new ObservableCollection<TimeInterval>();
            DateTime dt = Begin;
            while (dt.AddMinutes(duration) < End)
            {
                rt.Add(new TimeInterval() { Begin = dt, Interval = ts });
                dt = dt.AddMinutes(duration);
            }
            if (rt.Count > 0)
                rt.Add(new TimeInterval() { Begin = rt.Last().End, Interval = new TimeSpan(this.End.Ticks - rt.Last().End.Ticks) });
            else
                rt.Add(new TimeInterval() { Begin = this.Begin, Interval = new TimeSpan(this.End.Ticks - this.Begin.Ticks)});
            return rt;
        }

        public int CompareTo(TimeInterval other)
        {
            if (other == null) return 1;
            else if (this.Begin != other.Begin) return this.Begin.CompareTo(other.Begin);
            else return this.End.CompareTo(other.End);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class TimeInterval : INotifyPropertyChanged
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
            DateTime minDt, maxDt, midDt1, midDt2;
            if (i.Begin <= this.Begin)
            {
                minDt = i.Begin;
                midDt1 = this.Begin;
            }
            else
            {
                minDt = this.Begin;
                midDt1 = i.Begin;
            }
            if (i.End > this.End)
            {
                maxDt = i.End;
                midDt2 = this.End;
            }
            else
            {
                maxDt = this.End;
                midDt2 = i.End;
            }
            if ((midDt1 >= minDt && midDt1 <= maxDt) || (midDt2 >= minDt && midDt2 <= maxDt))
                return true;
            else
                return false;
        }
    }
}

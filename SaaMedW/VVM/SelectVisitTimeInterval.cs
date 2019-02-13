using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class SelectVisitTimeInterval : TimeInterval
    {
        public SelectDateIntervalsViewModel Parent { get; set; }
        public TypeTimeInterval TypeTimeInterv { get; set; }
        public bool IsVisit { get => TypeTimeInterv == TypeTimeInterval.Visit; }

        public SelectVisitTimeInterval() : base() { }
        public SelectVisitTimeInterval(TimeInterval timeInterval) : base(timeInterval)
        {
        }
        public SelectVisitTimeInterval(DateTime dt, int h1, int m1, int h2, int m2) :
            base(dt, h1, m1, h2, m2)
        { }
        //public RelayCommand SelectIntervalCommand
        //{
        //    get { return new RelayCommand(SelectIntervalProc, s => !IsVisit); }
        //}

        //private void SelectIntervalProc(object obj)
        //{
        //    //SelectIntervalViewModel root 
        //    //    = this.Parent
        //}
    }
}

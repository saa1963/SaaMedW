using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public enum TypeTimeInterval
    {
        Visit, Graphic
    }
    public class VisitTimeInterval : TimeInterval
    {
        public int PersonalId { get; set; }
        public DateTime Dt{ get; set; }
        public TypeTimeInterval typeTimeInterval { get; set; }

        public VisitTimeInterval():base() { }
        public VisitTimeInterval(TimeInterval timeInterval): base(timeInterval)
        {
        }
        public VisitTimeInterval(DateTime dt, int h1, int m1, int h2, int m2): 
            base(dt, h1, m1, h2, m2) { }

        //public RelayCommand AddVisit
        //{
        //    get { return new RelayCommand(AddVisitProc); }
        //}
        //private void AddVisitProc(object obj)
        //{
        //    //new Visit() { Dt = this.Begin, Duration = this.Interval.Minutes }
        //}
    }
}

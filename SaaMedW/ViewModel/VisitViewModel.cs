using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SaaMedW.ViewModel
{
    public class VisitViewModel: ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private DateTime SelectedDateNext { get => SelectedDate.AddDays(1); }
        public DateTime SelectedDate { get; set; }

        public ObservableCollection<VmVisit> ListVisit { get; set; }
            = new ObservableCollection<VmVisit>();
        public VisitViewModel():base()
        {
            SelectedDate = DateTime.Today;
            RefreshData();
        }

        private void RefreshData()
        {
            ListVisit.Clear();
            var q = ctx.Visit.Include(s => s.Person).Include(s => s.Personal)
                .Where(s => s.Dt >= SelectedDate && s.Dt < SelectedDateNext)
                .OrderBy(s => s.Dt);
            foreach(var o in q)
            {
                ListVisit.Add(new VmVisit(o));
            }
        }
    }
}

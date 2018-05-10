using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class PersonalVisitsViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();

        public int PersonalId { get; set; }
        public ObservableCollection<DateIntervalsViewModel> DateIntervals { get; set; }
         = new ObservableCollection<DateIntervalsViewModel>();

        public void Fill()
        {
            // Список дат из графика
            var personal = ctx.Personal.Find(PersonalId);
            var dates = personal.Graphic
                .Where(s => s.Dt >= DateTime.Today && s.PersonalId == PersonalId)
                .GroupBy(s => s.Dt)
                .Select(s => new DateIntervalsViewModel()
                    { Dt = s.Key, PersonalId = this.PersonalId })
                .OrderBy(s => s.Dt);
            foreach(var o in dates)
            {
                o.Fill();
                DateIntervals.Add(o);
            }
        }
    }
}

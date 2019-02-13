using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class PersonalVisitsViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();

        public EditVisitViewModel Parent { get; set; }
        public int PersonalId { get; set; }
        public string PersonalFio { get; set; }
        public Benefit Benefit { get; set; }
        public ObservableCollection<DateIntervals> DateIntervals { get; set; }
         = new ObservableCollection<DateIntervals>();

        public void Fill()
        {
            // Список дат из графика
            var personal = ctx.Personal.Find(PersonalId);
            var dates = personal.Graphic
                .Where(s => s.Dt >= DateTime.Today && s.PersonalId == PersonalId)
                .GroupBy(s => s.Dt)
                .Select(s => new DateIntervals()
                    { Dt = s.Key, PersonalId = this.PersonalId, Benefit = this.Benefit, Parent = this })
                .OrderBy(s => s.Dt);
            foreach(var o in dates)
            {
                if (o.Fill() > 0)
                    DateIntervals.Add(o);
            }
        }
    }
}

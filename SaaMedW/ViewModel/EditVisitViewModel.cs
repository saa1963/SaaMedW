using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;

namespace SaaMedW.ViewModel
{
    public class EditVisitViewModel : ViewModelBase
    {
        SaaMedEntities ctx = new SaaMedEntities();

        public ObservableCollection<Benefit> BenefitsList { get; set; } =
                new ObservableCollection<Benefit>();
        public ObservableCollection<PersonalVisitsViewModel> PersonalVisits { get; set; }
            = new ObservableCollection<PersonalVisitsViewModel>();
        public int SelectedBenefitId { get; set; }
        public EditVisitViewModel()
        {
            foreach(var o in ctx.Benefit.OrderBy(s => s.Name))
            {
                BenefitsList.Add(o);
            }
        }
        public RelayCommand RefreshGrid
        {
            get { return new RelayCommand(RefreshGridProc); }
        }
        private void RefreshGridProc(object obj)
        {
            PersonalVisits.Clear();
            int specialtyCurrent = BenefitsList.Single(s0 => s0.Id == SelectedBenefitId).SpecialtyId;
            foreach (var o in ctx.PersonalSpecialty.Include(s => s.Personal)
                .Where(s => s.SpecialtyId == specialtyCurrent)
                .Select(s => new PersonalVisitsViewModel()
                    { PersonalId = s.PersonalId, PersonalFio = s.Personal.Fio }))
            {
                o.Fill();
                if (o.DateIntervals.Count > 0)
                    PersonalVisits.Add(o);
            }
            OnPropertyChanged("PersonalVisits");
        }
    }
}

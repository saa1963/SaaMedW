using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows.Input;

namespace SaaMedW.ViewModel
{
    public class EditVisitViewModel : ViewModelBase
    {
        public SaaMedEntities ctx = new SaaMedEntities();

        public ObservableCollection<Benefit> BenefitsList { get; set; } =
                new ObservableCollection<Benefit>();
        public ObservableCollection<VmPerson> PersonList { get; set; } =
                new ObservableCollection<VmPerson>();
        private int m_SelectedPersonId;
        public int SelectedPersonId
        {
            get => m_SelectedPersonId;
            set
            {
                m_SelectedPersonId = value;
                OnPropertyChanged("IsSelectedPerson");
            }
        }
        public bool IsSelectedPerson { get => SelectedPersonId > 0; }
        public ObservableCollection<PersonalVisitsViewModel> PersonalVisits { get; set; }
            = new ObservableCollection<PersonalVisitsViewModel>();
        public int SelectedBenefitId { get; set; }
        public EditVisitViewModel()
        {
            foreach(var o in ctx.Benefit.OrderBy(s => s.Name))
            {
                BenefitsList.Add(o);
            }
            foreach (var o in ctx.Person.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .ThenBy(s => s.MiddleName))
            {
                PersonList.Add(new VmPerson(o));
            }
        }
        public RelayCommand RefreshGrid
        {
            get { return new RelayCommand(RefreshGridProc); }
        }
        private void RefreshGridProc(object obj)
        {
            PersonalVisits.Clear();
            Benefit curBenefit = BenefitsList.Single(s0 => s0.Id == SelectedBenefitId);
            int specialtyCurrent = curBenefit.SpecialtyId;
            foreach (var o in ctx.PersonalSpecialty.Include(s => s.Personal)
                .Where(s => s.SpecialtyId == specialtyCurrent)
                .Select(s => new PersonalVisitsViewModel()
                    { PersonalId = s.PersonalId, PersonalFio = s.Personal.Fio }))
            {
                o.Parent = this;
                o.Benefit = curBenefit;
                o.Fill();
                if (o.DateIntervals.Count > 0)
                    PersonalVisits.Add(o);
            }
            OnPropertyChanged("PersonalVisits");
        }
    }
}

using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System;
using SaaMedW.View;

namespace SaaMedW.ViewModel
{
    public class EditVisitViewModel : ViewModelBase
    {
        public SaaMedEntities ctx = new SaaMedEntities();
        private VmBenefit m_BenefitSel = null;
        public VmBenefit BenefitSel
        {
            get => m_BenefitSel;
            set
            {
                m_BenefitSel = value;
                OnPropertyChanged("BenefitSel");
            }
        }
        public ObservableCollection<EditPersonViewModel> PersonList { get; set; } =
                new ObservableCollection<EditPersonViewModel>();
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
        public EditVisitViewModel()
        {
            foreach (var o in ctx.Person.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .ThenBy(s => s.MiddleName))
            {
                PersonList.Add(new EditPersonViewModel(o));
            }
        }
        public RelayCommand RefreshGrid
        {
            get { return new RelayCommand(RefreshGridProc); }
        }
        private void RefreshGridProc(object obj)
        {
            if (BenefitSel == null) return;
            PersonalVisits.Clear();
            Benefit curBenefit = BenefitSel.Obj;
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
        public RelayCommand SelectBenefitCommand
        {
            get { return new RelayCommand(SelectBenefit); }
        }

        private void SelectBenefit(object obj)
        {
            var modelView = new SelectBenefitViewModel();
            var f = new SelectBenefitView() { DataContext = modelView};
            if (f.ShowDialog() ?? false)
            {
                BenefitSel = modelView.BenefitSel;
                RefreshGridProc(null);
            }
        }
    }
}

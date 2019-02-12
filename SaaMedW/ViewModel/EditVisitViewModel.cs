using System.Collections.ObjectModel;
using System.Linq;
using System.Data.Entity;
using System;
using SaaMedW.View;
using System.Collections.Generic;

namespace SaaMedW.ViewModel
{
    public class EditVisitViewModel : ViewModelBase
    {
        log4net.ILog log = log4net.LogManager.GetLogger(typeof(EditVisitViewModel));
        private bool m_IsOpen;
        public bool IsOpen
        {
            get => m_IsOpen;
            set
            {
                m_IsOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        private List<Specialty> lst;
        public ObservableCollection<VmSpecialty> SpecialtyList { get; } 
            = new ObservableCollection<VmSpecialty>();
        private void BuildTree(VmSpecialty sp)
        {
            sp.ChildSpecialties.Clear();
            foreach (var sp0 in lst.Where(s => s.ParentId == sp.Id)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                sp0.ParentSpecialty = sp;
                sp.ChildSpecialties.Add(sp0);
                BuildTree(sp0);
            }
            if (sp.ChildSpecialties.Count == 0)
            {
                foreach (var benefit in ctx.Benefit.Where(s => s.SpecialtyId == sp.Id))
                {
                    var o = new VmSpecialty()
                    {
                        Id = benefit.Id,
                        Name = benefit.Name,
                        ParentSpecialty = sp,
                        ParentId = sp.Id,
                        Cargo = SelectedItemMethod,
                        ReallyThisBenefit = true
                    };
                    sp.ChildSpecialties.Add(o);
                }
            }
        }

        private void SelectedItemMethod(VmSpecialty o)
        {
            if (o.ReallyThisBenefit)
            {
                BenefitSel = ctx.Benefit.Find(o.Id);
                IsOpen = false;
            }
            else
            {
                BenefitSel = null;
            }
            RefreshGridProc(null);
        }

        public SaaMedEntities ctx { get; set; } = new SaaMedEntities();
        private Benefit m_BenefitSel = null;
        public Benefit BenefitSel
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
            //ctx.Database.Log = s => log.Info(s);
            lst = ctx.Specialty.ToList();
            foreach (var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                SpecialtyList.Add(sp);
            }

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
            PersonalVisits.Clear();
            if (BenefitSel == null) return;
            Benefit curBenefit = BenefitSel;
            int specialtyCurrent = curBenefit.SpecialtyId;
            foreach (var o in ctx.PersonalSpecialty.Include(s => s.Personal)
                //.Where(s => s.SpecialtyId == specialtyCurrent)
                .Where(ПроверкаНаСпециальность)
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

        private bool ПроверкаНаСпециальность(PersonalSpecialty arg)
        {
            // это специальность к которой подвязана выбранная услуга
            Specialty specialty = ctx.Specialty.Find(BenefitSel.SpecialtyId);
            while (arg.SpecialtyId != specialty.Id && specialty.ParentId != null)
            {
                specialty = ctx.Specialty.Find(specialty.ParentId);
            }
            if (arg.SpecialtyId == specialty.Id) return true;
            else return false;
        }
    }
}

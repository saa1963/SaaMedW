using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SaaMedW
{
    public class EditOneVisitViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        private ObservableCollection<VmSpecialty> m_specialty
            = new ObservableCollection<VmSpecialty>();
        private List<Specialty> lst;
        private SaaMedEntities ctx;
        private enVisitStatus m_Status;

        public ObservableCollection<VmSpecialty> SpecialtyList { get => m_specialty; }
        public List<StatusName> ListStatus { get; set; } = new List<StatusName>();
        public bool IsEnabledOk
        {
            get
            {
                if (VisitBenefit.Count <= 0) return false;
                if (PersonalSel == null) return false;
                if (IntervalSel == null) return false;
                return true;
            }
            set { }
        }
        private int m_Duration;
        private VmPersonal _personalSel;
        private TimeInterval _intervalSel;

        public int Duration
        {
            get => m_Duration;
            set
            {
                m_Duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public VmPersonal PersonalSel
        {
            get => _personalSel;
            set
            {
                _personalSel = value;
                OnPropertyChanged("PersonalSel");
            }
        }
        public TimeInterval IntervalSel
        {
            get => _intervalSel;
            set
            {
                _intervalSel = value;
                OnPropertyChanged("IntervalSel");
            }
        }
        public EditOneVisitViewModel(SaaMedEntities _ctx, IEnumerable<int> sps = null)
        {
            ctx = _ctx;
            if (sps == null)
            {
                lst = ctx.Specialty.ToList();
            }
            else
            {
                lst = ctx.Specialty
                    .Where(s => sps.Contains(s.Id)).ToList();
            }
            foreach (var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                m_specialty.Add(sp);
            }
            ListStatus.Add(new StatusName() { Id = enVisitStatus.Предварительный, Name = "Предварительный" });
            ListStatus.Add(new StatusName() { Id = enVisitStatus.Завершен, Name = "Завершен" });

            VisitBenefit.CollectionChanged += VisitBenefit_CollectionChanged;
        }

        private void VisitBenefit_CollectionChanged(
            object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
            => OnPropertyChanged("IsEnabledOk");

        public EditOneVisitViewModel(SaaMedEntities _ctx, Visit visit)
            : this(_ctx,  visit.Personal.PersonalSpecialty.Select(s => s.SpecialtyId))
        {
            m_Duration = visit.Duration;
            m_Status = visit.Status;
            foreach (var o in visit.VisitBenefit)
            {
                VisitBenefit.Add(new VisitBenefit()
                { BenefitId = o.BenefitId, Benefit = ctx.Benefit.Find(o.BenefitId), Kol = o.Kol });
            }

        }
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
                SelectedBenefit1 = ctx.Benefit.Find(o.Id);
            }
            else
            {
                SelectedBenefit1 = null;
            }
        }
        /// <summary>
        /// Отобранные услуги
        /// </summary>
        public ObservableCollection<VisitBenefit> VisitBenefit { get; set; }
            = new ObservableCollection<VisitBenefit>();
        public Benefit SelectedBenefit1 { get; set; }
        public VisitBenefit SelectedBenefit2 { get; set; }
        public enVisitStatus Status
        {
            get => m_Status;
            set
            {
                m_Status = value;
                OnPropertyChanged("Status");
            }
        }
        public RelayCommand AddBenefitCommand
        {
            get => new RelayCommand(AddBenefit, s => SelectedBenefit1 != null
                && VisitBenefit.SingleOrDefault(s0 => s0.BenefitId == SelectedBenefit1.Id) == null);
        }

        private void AddBenefit(object obj)
        {
            VisitBenefit.Add(new VisitBenefit()
            { BenefitId = SelectedBenefit1.Id, Benefit = SelectedBenefit1, Kol = 1 });
            OnPropertyChanged("IsEnabledOk");
        }
        public RelayCommand RemoveBenefitCommand
        {
            get => new RelayCommand(RemoveBenefit, s => SelectedBenefit2 != null);
        }

        private void RemoveBenefit(object obj)
        {
            VisitBenefit.Remove(SelectedBenefit2);
            if (VisitBenefit.Count == 0)
            {
                PersonalSel = null;
                IntervalSel = null;
            }
        }

        public RelayCommand CalcDurationCommand
        {
            get => new RelayCommand(CalcDuration, s => VisitBenefit.Count > 0);
        }

        private void CalcDuration(object obj)
        {
            Duration = VisitBenefit.Sum(s => s.Benefit.Duration);
        }

        public RelayCommand SelectTimeCommand
        {
            get => new RelayCommand(SelectTime,
                s => VisitBenefit.Count > 0 && m_Duration > 0);
        }

        public string Error => "";

        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Duration":
                        if (Duration <= 0)
                            result = "Не верная продолжительность посещения";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        private void SelectTime(object obj)
        {
            var modelView
                = new SelectIntervalViewModel(VisitBenefit.Select(s => s.Benefit).ToList(), m_Duration);
            var f = new SelectInterval() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                PersonalSel = new VmPersonal(ctx.Personal.Find(modelView.ReturnInterval.Parent.Parent.PersonalId));
                IntervalSel = modelView.ReturnInterval;
                OnPropertyChanged("IsEnabledOk");
            }
        }
    }

    public class NameKol
    {
        public string Name { get; set; }
        public int Kol { get; set; }
    }

    public class StatusName
    {
        public enVisitStatus Id { get; set; }
        public string Name { get; set; }
    }
}

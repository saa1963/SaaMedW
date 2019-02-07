using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SaaMedW.ViewModel
{
    public class EditOneVisitViewModel: ViewModelBase
    {
        private ObservableCollection<VmSpecialty> m_specialty
            = new ObservableCollection<VmSpecialty>();
        private List<Specialty> lst;
        private SaaMedEntities ctx = new SaaMedEntities();
        private enVisitStatus m_Status;

        public ObservableCollection<VmSpecialty> SpecialtyList { get => m_specialty; }
        public List<StatusName> ListStatus { get; set; } = new List<StatusName>();
        public EditOneVisitViewModel()
        {
            lst = ctx.Specialty.ToList();
            foreach (var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                m_specialty.Add(sp);
            }
            ListStatus.Add(new StatusName() { Id = enVisitStatus.Предварительный, Name = "Предварительный" });
            ListStatus.Add(new StatusName() { Id = enVisitStatus.Завершен, Name = "Завершен" });
        }
        public EditOneVisitViewModel(Visit visit):this()
        {
            m_Status = visit.Status;
            foreach(var o in visit.VisitBenefit)
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
                { BenefitId = SelectedBenefit1.Id, Benefit = SelectedBenefit1, Kol = 1  });
        }
        public RelayCommand RemoveBenefitCommand
        {
            get => new RelayCommand(RemoveBenefit, s => SelectedBenefit2 != null);
        }

        private void RemoveBenefit(object obj)
        {
            VisitBenefit.Remove(SelectedBenefit2);
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

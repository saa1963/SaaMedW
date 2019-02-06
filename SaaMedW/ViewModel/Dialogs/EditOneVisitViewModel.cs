using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class EditOneVisitViewModel: ViewModelBase
    {
        private ObservableCollection<VmSpecialty> m_specialty
            = new ObservableCollection<VmSpecialty>();
        private List<Specialty> lst;
        private SaaMedEntities ctx = new SaaMedEntities();
        private int m_PersonId;
        private int m_PersonalId;
        private DateTime m_Dt;
        private int m_Duration;
        private int m_Status;

        public ObservableCollection<VmSpecialty> SpecialtyList { get => m_specialty; }
        public List<Person> ListPerson { get; private set; }
        public List<Personal> ListPersonal { get; private set; }
        public List<IdName> ListStatus { get; private set; } = new List<IdName>();
        public EditOneVisitViewModel()
        {
            lst = ctx.Specialty.ToList();
            foreach (var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                m_specialty.Add(sp);
            }
            ListPerson = ctx.Person.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .ThenBy(s => s.MiddleName).ToList();
            ListPersonal = ctx.Personal.Where(s => s.Active).OrderBy(s => s.Fio).ToList();
            ListStatus.Add(new IdName() { Id = 0, Name = "Предварительный" });
            ListStatus.Add(new IdName() { Id = 1, Name = "Завершен" });
        }
        public EditOneVisitViewModel(Visit visit):this()
        {
            m_PersonId = visit.PersonId;
            m_PersonalId = visit.PersonalId;
            m_Dt = visit.Dt;
            m_Duration = visit.Duration;
            m_Status = visit.Status;
            foreach(var o in visit.VisitBenefit)
            {
                VisitBenefit.Add(new VisitBenefit()
                    { BenefitId = o.BenefitId, Benefit = ctx.Benefit.Find(o.BenefitId), Kol = o.Kol });
                //ListBenefit.Remove(ListBenefit.Single(s => s.Id == o.BenefitId));
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
        public int PersonId
        {
            get => m_PersonId;
            set { m_PersonId = value; OnPropertyChanged("PersonId"); }
        }
        public int PersonalId
        {
            get => m_PersonalId;
            set { m_PersonalId = value; OnPropertyChanged("PersonalId"); }
        }
        public DateTime Dt
        {
            get => m_Dt.Date;
            set
            {
                m_Dt = value.Date.AddHours(m_Dt.Hour).AddMinutes(m_Dt.Minute);
                OnPropertyChanged("Dt");
            }
        }
        public int H1
        {
            get => m_Dt.Hour;
            set
            {
                m_Dt = m_Dt.Date.AddHours(value).AddMinutes(m_Dt.Minute);
                OnPropertyChanged("H1");
            }
        }
        public int M1
        {
            get => m_Dt.Minute;
            set
            {
                m_Dt = m_Dt.Date.AddHours(m_Dt.Hour).AddMinutes(value);
                OnPropertyChanged("M1");
            }
        }
        public int Duration
        {
            get => m_Duration;
            set { m_Duration = value; OnPropertyChanged("Duration"); }
        }
        public int Status
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
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SaaMedW
{
    class EditZakazViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        SaaMedEntities ctx = new SaaMedEntities();
        public EditZakazViewModel(int personId)
        {
            Person = ctx.Person.Find(personId);
            foreach (var p in ctx.Person.OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName).ThenBy(s => s.MiddleName)
                .ThenBy(s => s.BirthDate))
            {
                PersonList.Add(p);
            }
            foreach (var p in ctx.Personal.Where(s => s.Active).OrderBy(s => s.Fio))
            {
                PersonalList.Add(new VmPersonal(p));
            }
            RefreshDmsCompanies();
            m_Zakaz1List = new ObservableCollection<BenefitForZakaz>();
        }
        private ObservableCollection<Person> m_PersonList = new ObservableCollection<Person>();
        public ObservableCollection<Person> PersonList
        {
            get => m_PersonList;
            set
            {
                m_PersonList = value;
                OnPropertyChanged("PersonList");
            }
        }
        private ObservableCollection<VmPersonal> m_PersonalList = new ObservableCollection<VmPersonal>();
        public ObservableCollection<VmPersonal> PersonalList
        {
            get => m_PersonalList;
            set
            {
                m_PersonalList = value;
                OnPropertyChanged("PersonalList");
            }
        }
        private ObservableCollection<DmsCompany> m_DmsCompanyList = new ObservableCollection<DmsCompany>();
        public ObservableCollection<DmsCompany> DmsCompanyList
        {
            get => m_DmsCompanyList;
            set
            {
                m_DmsCompanyList = value;
                OnPropertyChanged("DmsCompanyList");
            }
        }
        private ObservableCollection<BenefitForZakaz> m_Zakaz1List
            = new ObservableCollection<BenefitForZakaz>();
        //public ObservableCollection<BenefitForZakaz> m_Zakaz1List
        //{
        //    get => mm_Zakaz1List;
        //    set
        //    {
        //        mm_Zakaz1List = value;
        //        OnPropertyChanged("m_Zakaz1List");
        //    }
        //}
        public ICollectionView Zakaz1List
        {
            get
            {
                return CollectionViewSource.GetDefaultView(m_Zakaz1List);
            }
        }
        private int m_Num;
        public int Num
        {
            get => m_Num;
            set
            {
                m_Num = value;
                OnPropertyChanged("Num");
            }
        }
        private DateTime m_Dt;
        public DateTime Dt
        {
            get => m_Dt;
            set
            {
                m_Dt = value;
                OnPropertyChanged("Dt");
            }
        }
        private Person m_Person;
        public Person Person
        {
            get => m_Person;
            set
            {
                m_Person = value;
                OnPropertyChanged("Person");
            }
        }
        private bool m_Dms;
        public bool Dms
        {
            get => m_Dms;
            set
            {
                m_Dms = value;
                OnPropertyChanged("Dms");
            }
        }
        private string m_Polis;
        public string Polis
        {
            get => m_Polis;
            set
            {
                m_Polis = value;
                OnPropertyChanged("Polis");
            }
        }
        private DmsCompany m_DmsCompany;
        public DmsCompany DmsCompany
        {
            get => m_DmsCompany;
            set
            {
                m_DmsCompany = value;
                OnPropertyChanged("DmsCompany");
            }
        }
        public RelayCommand DmsCompanyClassificatorCommand
            => new RelayCommand(DmsCompanyClassificatorOpen);

        private void DmsCompanyClassificatorOpen(object obj)
        {
            var modelView = new DmsCompanyViewModel(ctx);
            var f = new DmsCompanyView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                RefreshDmsCompanies();
                var o = modelView.View.CurrentItem as VmDmsCompany;
                DmsCompany = o.Obj;
            }
        }

        private void RefreshDmsCompanies()
        {
            DmsCompanyList.Clear();
            foreach (var p in ctx.DmsCompany.OrderBy(s => s.Name))
            {
                DmsCompanyList.Add(p);
            }
        }

        private decimal m_Sm = 0;
        public decimal Sm
        {
            get => m_Sm;
            set
            {
                m_Sm = value;
                OnPropertyChanged("Sm");
            }
        }

        public RelayCommand AddBenefitCommand
            => new RelayCommand(AddBenefit);

        private void AddBenefit(object obj)
        {
            var viewModel = new SelectSpecialtyViewModel();
            var f = new SelectSpecialtyView() { DataContext = viewModel };
            if (f.ShowDialog() ?? false)
            {
                var o = new BenefitForZakaz()
                {
                    BenefitId = viewModel.BenefitSel.Id,
                    BenefitName = viewModel.BenefitSel.Name,
                    Kol = 1,
                    Price = viewModel.BenefitSel.Price,
                    Sum = SetSum
                };
                m_Zakaz1List.Add(o);
                Zakaz1List.MoveCurrentToLast();
                RefreshItogo();
            }
        }

        private void SetSum()
        {
            RefreshItogo();
        }

        private void RefreshItogo()
        {
            decimal sm = 0;
            foreach(var o in m_Zakaz1List)
            {
                sm += o.Kol * o.Price;
            }
            Sm = sm;
        }

        public RelayCommand AddEmptyBenefitCommand
            => new RelayCommand(AddEmptyBenefit);

        private void AddEmptyBenefit(object obj)
        {
            var o = new BenefitForZakaz()
            {
                Kol = 1,
            };
            m_Zakaz1List.Add(o);
            Zakaz1List.MoveCurrentToLast();
            RefreshItogo();
        }

        public RelayCommand DelBenefitCommand
            => new RelayCommand(DelBenefit, s => Zakaz1List.CurrentItem != null);

        private void DelBenefit(object obj)
        {
            var benefit = Zakaz1List.CurrentItem as BenefitForZakaz;
            Sm -= benefit.Kol * benefit.Price;
            m_Zakaz1List.Remove(benefit);
            Zakaz1List.MoveCurrentToPrevious();
            RefreshItogo();
        }

        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        //if (String.IsNullOrWhiteSpace(Name))
                        //    result = "Не введено наименование.";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public string Error => "";
    }

    internal class BenefitForZakaz: NotifyPropertyChanged
    {
        public Action Sum { get; set; }
        private int? m_BenefitId;
        public int? BenefitId
        {
            get => m_BenefitId;
            set
            {
                m_BenefitId = value;
                OnPropertyChanged("BenefitId");
            }
        }
        private int m_PersonalId;
        public int PersonalId
        {
            get => m_PersonalId;
            set
            {
                m_PersonalId = value;
                OnPropertyChanged("PersonalId");
            }
        }
        private decimal m_Price;
        public decimal Price
        {
            get => m_Price;
            set
            {
                m_Price = value;
                OnPropertyChanged("Price");
                OnPropertyChanged("Sm");
                Sum?.Invoke();
            }
        }
        private int m_Kol;
        public int Kol
        {
            get => m_Kol;
            set
            {
                m_Kol = value;
                OnPropertyChanged("Kol");
                OnPropertyChanged("Sm");
                Sum?.Invoke();
            }
        }
        private string m_BenefitName;
        public string BenefitName
        {
            get => m_BenefitName;
            set
            {
                m_BenefitName = value;
                OnPropertyChanged("BenefitName");
            }
        }
        public decimal Sm
        {
            get => m_Price * m_Kol;
            set {}
        }
    }
}

using SaaMedW.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SaaMedW
{
    public class EditZakazViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        SaaMedEntities ctx = new SaaMedEntities();
        private bool m_NewMode;
        public bool NewMode
        {
            get => m_NewMode;
            set
            {
                m_NewMode = value;
                OnPropertyChanged("NewMode");
                OnPropertyChanged("EditMode");
            }
        }
        public bool EditMode
        {
            get => !m_NewMode;
            set
            {
                OnPropertyChanged("EditMode");
            }
        }

        private Zakaz zakaz;
        public EditZakazViewModel(int zakazId)
        {
            zakaz = ctx.Zakaz.Find(zakazId);
            Person = zakaz.Person;
            InitPerson();
            InitPersonal();
            RefreshDmsCompanies();
            DmsCompany = zakaz.DmsCompany;
            Num = zakaz.Num;
            Dt = zakaz.Dt;
            Dms = zakaz.Dms;
            Polis = zakaz.Polis;

            Zakaz1List = new ObservableCollection<BenefitForZakaz>();
            foreach (var z1 in zakaz.Zakaz1)
            {
                Zakaz1List.Add(new BenefitForZakaz()
                {
                     zakaz1Id = z1.Id,
                     BenefitId = z1.BenefitId,
                     BenefitName = z1.BenefitName,
                     Kol = z1.Kol,
                     PersonalId = z1.PersonalId,
                     Price = z1.Price,
                     RootSpecialty = z1.SpecialtyRootId,
                     Sm = z1.Kol * z1.Price,
                     Sum = SetSum,
                     PersonalList = PersonalList.Where(s => s.SpecialtyId == z1.SpecialtyRootId).ToList()
                });
            }
            NewMode = false;
        }
        public EditZakazViewModel(int personId, int? dmscompanyId)
        {
            Person = ctx.Person.Find(personId);
            InitPerson();
            InitPersonal();
            RefreshDmsCompanies();
            DmsCompany = ctx.DmsCompany.Find(dmscompanyId);
            Zakaz1List = new ObservableCollection<BenefitForZakaz>();
            NewMode = true;
        }

        private void InitPerson()
        {
            
            foreach (var p in ctx.Person.OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName).ThenBy(s => s.MiddleName)
                .ThenBy(s => s.BirthDate))
            {
                PersonList.Add(p);
            }
        }

        private void InitPersonal()
        {
            foreach (var p in ctx.Personal.Where(s => s.Active).OrderBy(s => s.Fio))
            {
                foreach (var p1 in p.PersonalSpecialty)
                {
                    PersonalList.Add(new PersonalForZakaz1()
                    {
                        Id = p.Id,
                        Name = p.Fio,
                        SpecialtyId = Specialty.RootId(ctx, p1.SpecialtyId)
                    });
                }
            }
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
        private ObservableCollection<PersonalForZakaz1> m_PersonalList = new ObservableCollection<PersonalForZakaz1>();
        public ObservableCollection<PersonalForZakaz1> PersonalList
        {
            get => m_PersonalList;
            set
            {
                m_PersonalList = value;
                OnPropertyChanged("PersonalList");
            }
        }
        public ICollectionView PersonalListView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(PersonalList);
            }
            set { }
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
        public ObservableCollection<BenefitForZakaz> Zakaz1List { get; set; }
            = new ObservableCollection<BenefitForZakaz>();

        public ICollectionView Zakaz1ListView
        {
            get
            {
                return CollectionViewSource.GetDefaultView(Zakaz1List);
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

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

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
                var rootSpecialty = Specialty.RootId(ctx, viewModel.SpecialtySel.Id);
                var o = new BenefitForZakaz()
                {
                    BenefitId = viewModel.BenefitSel.Id,
                    BenefitName = viewModel.BenefitSel.Name,
                    Kol = 1,
                    Price = viewModel.BenefitSel.Price,
                    Sum = SetSum,
                    RootSpecialty = rootSpecialty,
                    PersonalList = PersonalList.Where(s => s.SpecialtyId == rootSpecialty).ToList()
                };
                Zakaz1List.Add(o);
                Zakaz1ListView.MoveCurrentToLast();
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
            foreach (var o in Zakaz1List)
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
                Sum = SetSum
            };
            Zakaz1List.Add(o);
            Zakaz1ListView.MoveCurrentToLast();
            RefreshItogo();
        }

        public RelayCommand DelBenefitCommand
            => new RelayCommand(DelBenefit, s => Zakaz1ListView.CurrentItem != null);

        private void DelBenefit(object obj)
        {
            var benefit = Zakaz1ListView.CurrentItem as BenefitForZakaz;
            Sm -= benefit.Kol * benefit.Price;
            Zakaz1List.Remove(benefit);
            Zakaz1ListView.MoveCurrentToPrevious();
            RefreshItogo();
        }

        public bool IsOpenPrint { get; set; }

        public RelayCommand PrintDogovorCommand
        {
            get => new RelayCommand(PrintDogovor);
        }

        private void PrintDogovor(object obj)
        {
            Print.PrintDogovor(Dt, Num, Person, IsOpenPrint);
        }

        public RelayCommand PrintVmeshCommand
        {
            get => new RelayCommand(PrintVmesh);
        }

        private void PrintVmesh(object obj)
        {
            Print.PrintVmesh(Dt, Person, IsOpenPrint);
        }

        public RelayCommand PrintMedcardCommand
        {
            get => new RelayCommand(PrintMedcard);
        }

        private void PrintMedcard(object obj)
        {
            Print.PrintMedcard(Person, IsOpenPrint);
        }

        public RelayCommand ZakazReportCommand
        {
            get => new RelayCommand(PrintZakazReport, s => ValidZakaz());
        }

        private void PrintZakazReport(object obj)
        {
            if (!ValidZakaz()) return;
            Print.ZakazReport(Dt, Num, Person, Dms, Zakaz1List, IsOpenPrint);
        }

        public RelayCommand PrintAllCommand
        { 
            get => new RelayCommand(PrintAll, s => ValidZakaz());
        }
        public void PrintAll(object obj)
        {
            if (!ValidZakaz()) return;
            Print.PrintDogovor(Dt, Num, Person, IsOpenPrint);
            Print.PrintVmesh(Dt, Person, IsOpenPrint);
            Print.PrintMedcard(Person, IsOpenPrint);
            Print.ZakazReport(Dt, Num, Person, Dms, Zakaz1List, IsOpenPrint);
        }

        private bool m_NotPayed = true;
        public bool NotPayed
        {
            get => m_NotPayed;
            set
            {
                m_NotPayed = value;
                OnPropertyChanged("NotPayed");
            }
        }
#if (!DEBUG)
        public RelayCommand PayCommand
        {
            get => new RelayCommand(Pay, s => ServiceLocator.Instance.GetService<IKkm>().IsInitialized && ValidZakaz());
        }
        //public RelayCommand PayCommand { get; set; }
        //    = new RelayCommand(Pay, s => ServiceLocator.Instance.GetService<IKkm>().IsInitialized);
#else
        public RelayCommand PayCommand
        {
            get => new RelayCommand(Pay, s => ValidZakaz());
        }
            
#endif
        private void Pay(object obj)
        {
            //var o = obj as EditZakazViewModel;
            if (!ValidZakaz())
            {
                MessageBox.Show(ValidZakazResult);
                return;
            }
            if (!Dms)
            {
                List<Tuple<string, int, decimal>> uslugi = new List<Tuple<string, int, decimal>>();
                IKkm kkm = ServiceLocator.Instance.GetService<IKkm>();
                var viewModel = new PayInvoiceViewModel() { КОплате = Sm };
                var f = new PayInvoiceView() { DataContext = viewModel };
                if (f.ShowDialog() ?? false)
                {
                    foreach (var o1 in Zakaz1List)
                    {
                        uslugi.Add(new Tuple<string, int, decimal>(o1.BenefitName, o1.Kol, o1.Price));
                    }
#if (!DEBUG)
                    if (kkm.Register(uslugi, viewModel.Sm, viewModel.PaymentType, viewModel.Email, viewModel.IsElectronic))
#else
                    if (true)
#endif
                    {
                        NotPayed = false;
                        Save(viewModel.PaymentType, viewModel.Email);
                        if (viewModel.IsElectronic)
                            MessageBox.Show("Электронный чек сформирован.");
                        CloseDialog = true;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка регистрации кассового чека.");
                    }
                }
            }
            else
            {
                NotPayed = false;
                Save(null, "");
                CloseDialog = true;
            }
        }

        private void Save(enumPaymentType? pt, string email)
        {
            var zakaz = new Zakaz()
            {
                Dms = this.Dms,
                DmsCompany = this.DmsCompany,
                DmsCompanyId = this.DmsCompany?.Id,
                Dt = this.Dt,
                Num = this.Num,
                Person = this.Person,
                PersonId = this.Person.Id,
                Polis = this.Polis,
                Email = email,
                Card = false
            };
            if (pt.HasValue && pt.Value == enumPaymentType.Безналичные)
            {
                zakaz.Card = true;
            }
            foreach (var o1 in this.Zakaz1List)
            {
                var zakaz1 = new Zakaz1()
                {
                    BenefitId = o1.BenefitId,
                    BenefitName = o1.BenefitName,
                    Kol = o1.Kol,
                    PersonalId = o1.PersonalId,
                    Price = o1.Price,
                    SpecialtyRootId = o1.RootSpecialty
                };
                zakaz.Zakaz1.Add(zakaz1);
            }
            this.ctx.Zakaz.Add(zakaz);
            this.ctx.SaveChanges();
            Options.SetParameter<int>(enumParameterType.Номер_договора, Num + 1);
        }

        private string ValidZakazResult;

        public bool ValidZakaz()
        {
            string result = null;
            bool rt = true;
            if (Num <= 0)
            {
                result = "Неверный номер заказа";
            }
            else if (Dms && String.IsNullOrWhiteSpace(Polis))
            {
                result = "Не введен номер полиса";
            }
            else if (Dms && DmsCompany == null)
            {
                result = "Не введена страховая компания";
            }
            else if (Zakaz1List.Count == 0)
            {
                result = "Не введены услуги";
            } else if (!ValidBenefits(out string res))
            {
                result = res;
            }
            if (result != null)
            {
                ValidZakazResult = result;
                rt = false;
            }
            else
            {
                ValidZakazResult = null;
            }
            return rt;
        }

        private bool ValidBenefits(out string res)
        {
            bool rt = true;
            res = null;
            foreach(var o in Zakaz1List)
            {
                if (String.IsNullOrWhiteSpace(o.BenefitName))
                {
                    res = "Не введено наименование услуги";
                    rt = false;
                    break;
                }
                else if (o.PersonalId <= 0)
                {
                    res = "Не введен врач";
                    rt = false;
                    break;
                }
                else if (o.Kol <= 0)
                {
                    res = "Не введено количество";
                    rt = false;
                    break;
                }
                else if (o.Price <= 0)
                {
                    res = "Не введена цена";
                    rt = false;
                    break;
                }
            }
            return rt;
        }

        private bool? m_CloseDialog;
        public bool? CloseDialog
        {
            get => m_CloseDialog;
            set
            {
                m_CloseDialog = value;
                OnPropertyChanged("CloseDialog");
            }
        }

        public RelayCommand SaveCommand
        {
            get => new RelayCommand(Save, s => ValidZakaz());
        }

        private void Save(object obj)
        {
            zakaz.Polis = Polis;
            zakaz.DmsCompany = DmsCompany;
            foreach(var z1 in zakaz.Zakaz1)
            {
                z1.PersonalId = Zakaz1List.Single(s => s.zakaz1Id == z1.Id).PersonalId;
            }
            ctx.SaveChanges();
            CloseDialog = true;
        }

        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Num":
                        if (Num <= 0) result = "Неверный номер заказа";
                        break;
                    case "Polis":
                        if (Dms && String.IsNullOrWhiteSpace(Polis))
                            result = "Не введен полис";
                        break;
                    case "DmsCompany":
                        if (Dms && DmsCompany == null)
                            result = "Не введена компания";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public string Error => "";
    }

    public class BenefitForZakaz: NotifyPropertyChanged
    {
        public int zakaz1Id { get; set; }
        public Action Sum { get; set; }
        private int m_BenefitId;
        public int BenefitId
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
        private List<PersonalForZakaz1> m_PersonalList;
        public List<PersonalForZakaz1> PersonalList
        {
            get => m_PersonalList;
            set
            {
                m_PersonalList = value;
                OnPropertyChanged("PersonalList");
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
        private int m_RootSpecialty;
        public int RootSpecialty
        {
            get => m_RootSpecialty;
            set
            {
                m_RootSpecialty = value;
                OnPropertyChanged("RootSpecialty");
            }
        }
    }

    public class PersonalForZakaz1: NotifyPropertyChanged
    {
        private int m_Id;
        public int Id
        {
            get => m_Id;
            set
            {
                m_Id = value;
                OnPropertyChanged("Id");
            }
        }
        private string m_Name;
        public string Name
        {
            get => m_Name;
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }
        private int m_SpecialtyId;
        public int SpecialtyId
        {
            get => m_SpecialtyId;
            set
            {
                m_SpecialtyId = value;
                OnPropertyChanged("SpecialtyId");
            }
        }
    }
}

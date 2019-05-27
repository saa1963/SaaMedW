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
                Sum = SetSum
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

        public bool IsOpenPrint { get; set; }

        public RelayCommand PrintDogovorCommand { get; set; }
            = new RelayCommand(PrintDogovor, IsPrintDovogor);

        private static bool IsPrintDovogor(object obj)
        {
            return true;
        }

        private static void PrintDogovor(object obj)
        {
            var o = obj as EditZakazViewModel;
            var fname = new Dogovor().GetDogovorFname(o.Dt, o.Num, o.Person);
            if (!o.IsOpenPrint)
            {
                o.HardCopy(fname);
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        public RelayCommand PrintVmeshCommand { get; set; }
            = new RelayCommand(PrintVmesh, IsPrintVmesh);

        private static bool IsPrintVmesh(object obj)
        {
            return true;
        }

        private static void PrintVmesh(object obj)
        {
            var o = obj as EditZakazViewModel;
            var fname = new Vmesh().DoIt(o.Dt, o.Person);
            if (!o.IsOpenPrint)
            {
                o.HardCopy(fname);
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        public RelayCommand PrintMedcardCommand { get; set; }
            = new RelayCommand(PrintMedcard, IsPrintMedcard);

        private static bool IsPrintMedcard(object obj)
        {
            return true;
        }

        private static void PrintMedcard(object obj)
        {
            var o = obj as EditZakazViewModel;
            var fname = new MedCard().DoIt(o.Person);
            if (!o.IsOpenPrint)
            {
                o.HardCopy(fname);
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        public RelayCommand ZakazReportCommand { get; set; }
            = new RelayCommand(ZakazReport);

        private static void ZakazReport(object obj)
        {
            var o = obj as EditZakazViewModel;
            if (!o.ValidZakaz()) return;
            var fname = new ZakazReport().DoIt(o.Dt, o.Num, o.Person, o.Dms, o.m_Zakaz1List);
            if (!o.IsOpenPrint)
            {
                try
                {
                    o.HardCopy(fname);
                }
                catch(Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                System.Diagnostics.Process.Start(fname);
            }
        }

        public RelayCommand PrintAllCommand { get; set; }
            = new RelayCommand(PrintAll);

        private static void PrintAll(object obj)
        {
            var o = obj as EditZakazViewModel;
            if (!o.ValidZakaz()) return;
            var fname = new Dogovor().GetDogovorFname(o.Dt, o.Num, o.Person);
            o.HardCopy(fname);
            fname = new Vmesh().DoIt(o.Dt, o.Person);
            o.HardCopy(fname);
            fname = new MedCard().DoIt(o.Person);
            o.HardCopy(fname);
            fname = new ZakazReport().DoIt(o.Dt, o.Num, o.Person, o.Dms, o.m_Zakaz1List);
            o.HardCopy(fname);
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
        public RelayCommand PayCommand { get; set; }
            = new RelayCommand(Pay, s => ServiceLocator.Instance.GetService<IKkm>().IsInitialized);
#else
        public RelayCommand PayCommand { get; set; }
            = new RelayCommand(Pay);
#endif
        private static void Pay(object obj)
        {
            var o = obj as EditZakazViewModel;
            if (!o.ValidZakaz()) return;
            List<Tuple<string, int, decimal>> uslugi = new List<Tuple<string, int, decimal>>();
            IKkm kkm = ServiceLocator.Instance.GetService<IKkm>();
            var viewModel = new PayInvoiceViewModel() { КОплате = o.Sm };
            var f = new PayInvoiceView() { DataContext = viewModel };
            if (f.ShowDialog() ?? false)
            {
                foreach (var o1 in o.m_Zakaz1List)
                {
                    uslugi.Add(new Tuple<string, int, decimal>(o1.BenefitName, o1.Kol, o1.Price));
                }
#if (!DEBUG)
  ...           if (kkm.Register(uslugi, viewModel.Sm, viewModel.PaymentType, viewModel.Email, viewModel.IsElectronic))
#else
                if (true)
#endif
                {
                    o.NotPayed = false;
                    o.Save();
                    if (viewModel.IsElectronic)
                        MessageBox.Show("Электронный чек сформирован.");
                    o.CloseDialog = true;
                }
                else
                {
                    MessageBox.Show("Ошибка регистрации кассового чека.");
                }
            }
        }

        private void Save()
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
            };
            foreach (var o1 in this.m_Zakaz1List)
            {
                var zakaz1 = new Zakaz1()
                {
                    BenefitId = o1.BenefitId,
                    BenefitName = o1.BenefitName,
                    Kol = o1.Kol,
                    PersonalId = o1.PersonalId,
                    Price = o1.Price
                };
                zakaz.Zakaz1.Add(zakaz1);
            }
            this.ctx.Zakaz.Add(zakaz);
            this.ctx.SaveChanges();
        }

        private void HardCopy(string fname)
        {
            ProcessStartInfo info = new ProcessStartInfo(fname);
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            var process = Process.Start(info);
            process.WaitForExit(10000);
        }

        private bool ValidZakaz()
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
            else if (m_Zakaz1List.Count == 0)
            {
                result = "Не введены услуги";
            } else if (!ValidBenefits(out string res))
            {
                result = res;
            }
            if (result != null)
            {
                MessageBox.Show(result);
                rt = false;
            }
            return rt;
        }

        private bool ValidBenefits(out string res)
        {
            bool rt = true;
            res = null;
            foreach(var o in m_Zakaz1List)
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

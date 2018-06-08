using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class EditInvoiceViewModel : ViewModelBase, IDataErrorInfo
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private DateTime _dt = DateTime.Today;
        private int _num;
        private enStatusInvoice _status = enStatusInvoice.Неоплачен;
        private int _personId;
        private decimal _sm;
        private Person _person;
        private ObservableCollection<VmInvoiceDetail> _listInvoiceDetail
            = new ObservableCollection<VmInvoiceDetail>();
        private int _id;

        public EditInvoiceViewModel()
        {
            ListPerson = ctx.Person.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .ThenBy(s => s.MiddleName).ToList();
        }
        public EditInvoiceViewModel(VmInvoice invoice):this()
        {
            Dt = invoice.Dt;
            Status = (enStatusInvoice)invoice.Status;
            PersonId = invoice.PersonId;
            Sm = invoice.Sm;
            Person = invoice.Person;
            foreach (var o in invoice.InvoiceDetail)
            {
                ListInvoiceDetail.Add(new VmInvoiceDetail()
                {
                    BenefitName = o.BenefitName,
                    Kol = o.Kol,
                    Price = o.Price,
                    Sm = o.Sm
                });
            }
        }

        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "PersonId":
                        if (PersonId <= 0)
                            result = "Не выбран пациент";
                        break;
                    case "ListInvoiceDetail":
                        if (ListInvoiceDetail.Count == 0)
                        {
                            result = "Не введены услуги";
                        }
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public int Id { get => _id; set => _id = value; }
        public DateTime Dt
        {
            get => _dt; set
            {
                _dt = value;
                OnPropertyChanged("Dt");
            }
        }
        public int Num
        {
            get => _num; set
            {
                _num = value;
                OnPropertyChanged("Num");
            }
        }
        public enStatusInvoice Status
        {
            get => _status; set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }
        public int PersonId
        {
            get => _personId; set
            {
                _personId = value;
                OnPropertyChanged("PersonId");
            }
        }
        public decimal Sm
        {
            get => _sm; set
            {
                _sm = value;
                OnPropertyChanged("Sm");
            }
        }
        public Person Person
        {
            get => _person; set
            {
                _person = value;
                OnPropertyChanged("Person");
            }
        }
        public ObservableCollection<VmInvoiceDetail> ListInvoiceDetail
        {
            get => _listInvoiceDetail; set
            {
                _listInvoiceDetail = value;
                OnPropertyChanged("ListInvoiceDetail");
            }
        }
        public string DateNumSum => "Счет № " + (Id > 0 ? Id.ToString() : "б/н") +
                    " от " + Dt.ToString("dd.MM.yyyy") + " на сумму " + Sm.ToString("0.00");
        public List<Person> ListPerson { get; set; }
        public RelayCommand AddBenefitCommand
        {
            get => new RelayCommand(AddBenefit);
        }

        public string Error => String.Empty;

        private void AddBenefit(object obj)
        {
            var viewModel = new SelectSpecialtyViewModel();
            var f = new SelectSpecialtyView() { DataContext = viewModel };
            if (f.ShowDialog() ?? false)
            {
                var o = new VmInvoiceDetail()
                {
                    BenefitName = viewModel.BenefitSel.Name,
                    Kol = 1,
                    Price = viewModel.BenefitSel.Price,
                    Sm = viewModel.BenefitSel.Price
                };
                Sm += o.Sm;
                ListInvoiceDetail.Add(o);
                OnPropertyChanged("ListInvoiceDetail");
                OnPropertyChanged("DateNumSum");
            }
        }
    }
}

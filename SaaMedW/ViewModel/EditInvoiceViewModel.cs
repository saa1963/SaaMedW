using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class EditInvoiceViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private VmVisit selectedVisit;
        private DateTime _dt;
        private int _num;
        private enStatusInvoice _status;
        private int _personId;
        private decimal _sm;
        private Person _person;
        private ObservableCollection<VmInvoiceDetail> _listInvoiceDetail
            = new ObservableCollection<VmInvoiceDetail>();
        private int _id;

        public EditInvoiceViewModel(VmVisit selectedVisit)
        {
            this.selectedVisit = selectedVisit;
            Dt = DateTime.Now;
            Status = enStatusInvoice.Неоплачен;
            PersonId = selectedVisit.PersonId;
            Sm = selectedVisit.VisitBenefit.Sum(s => s.Kol * s.Benefit.Price);
            Person = selectedVisit.Person;
            foreach (var o in selectedVisit.VisitBenefit)
            {
                ListInvoiceDetail.Add(new VmInvoiceDetail()
                {
                    BenefitName = o.Benefit.Name,
                    Kol = o.Kol,
                    Price = o.Benefit.Price,
                    Sm = o.Kol * o.Benefit.Price
                });
            }
            ListPerson = ctx.Person.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .ThenBy(s => s.MiddleName).ToList();
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
    }
}

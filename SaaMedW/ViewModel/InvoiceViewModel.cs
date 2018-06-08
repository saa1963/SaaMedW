using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SaaMedW.View;

namespace SaaMedW.ViewModel
{
    public class InvoiceViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        #region InvoiceList
        private ObservableCollection<VmInvoice> m_lst
            = new ObservableCollection<VmInvoice>();
        public ObservableCollection<VmInvoice> InvoiceList
        {
            get => m_lst;
            set { m_lst = value; OnPropertyChanged("InvoiceList"); }
        }
        public VmInvoice InvoiceSel { get; set; }
        #endregion
        #region StatusList
        public List<IdName> StatusList { get; set; }
            = new List<IdName> {
                new IdName { Id = -1, Name = "Все"},
                new IdName { Id = 0, Name = "Неоплаченные" },
                new IdName { Id = 1, Name = "Частично оплаченные" },
                new IdName { Id = 2, Name = "Оплаченные" } };
        public int StatusSel { get; set; } = -1;
        #endregion
        #region Dt1 - Dt2
        private DateTime m_Dt1 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DateTime Dt1
        {
            get => m_Dt1;
            set
            {
                m_Dt1 = value;
                OnPropertyChanged("Dt1");
                RefreshData();
            }
        }
        private DateTime m_Dt2 = DateTime.Today;
        public DateTime Dt2
        {
            get => m_Dt2;
            set { m_Dt2 = value; OnPropertyChanged("Dt2"); }
        }
        #endregion
        #region PersonList
        private List<VmPerson> m_personlist;
        public List<VmPerson> PersonList
        {
            get => m_personlist;
            set { m_personlist = value; OnPropertyChanged("PersonList"); }
        }
        private int m_PersonSel;
        public int PersonSel
        {
            get => m_PersonSel;
            set { m_PersonSel = value; OnPropertyChanged("PersonSel"); }
        } 
        #endregion
        public InvoiceViewModel()
        {
            PersonList = ctx.Person.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .ThenBy(s => s.MiddleName)
                .Select(s => new VmPerson() {Obj = s}).ToList();
            RefreshData();
        }

        private void RefreshData()
        {
            foreach(var o in ctx.Invoice
                .Include(s => s.InvoiceDetail)
                .Include(s => s.Person)
                .Where(s => s.Dt >= Dt1 && s.Dt <= Dt2 
                    && (PersonSel != 0 ? s.PersonId == PersonSel : true)
                    && (StatusSel != -1 ? s.Status == StatusSel : true)))
            {
                InvoiceList.Add(new VmInvoice(o));
            }
        }
        public RelayCommand ClearPersonSelCommand
        {
            get => new RelayCommand(ClearPersonSel, s => PersonSel > 0);
        }

        private void ClearPersonSel(object obj)
        {
            PersonSel = 0;
        }

        public RelayCommand AddCommand
        {
            get => new RelayCommand(AddInvoice);
        }

        private void AddInvoice(object obj)
        {
            var modelView = new EditInvoiceViewModel();
            var f = new EditInvoiceView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                var invoice = new VmInvoice()
                {
                    Dt = modelView.Dt,
                    Person = ctx.Person.Find(modelView.PersonId),
                    Sm = modelView.Sm,
                    Status = (int)modelView.Status
                };
                foreach (var o in modelView.ListInvoiceDetail)
                {
                    invoice.InvoiceDetail.Add(o.Obj);
                }
                InvoiceList.Add(invoice);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using SaaMedW.View;
using System.Diagnostics;

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
        private int m_StatusSel = -1;
        public int StatusSel
        {
            get => m_StatusSel;
            set
            {
                m_StatusSel = value;
                OnPropertyChanged("StatusSel");
                RefreshData();
            }
        }
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
            set
            {
                m_Dt2 = value;
                OnPropertyChanged("Dt2");
                RefreshData();
            }
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
            set
            {
                m_PersonSel = value;
                OnPropertyChanged("PersonSel");
                RefreshData();
            }
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
            InvoiceList.Clear();
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
                ctx.Invoice.Add(invoice.Obj);
                ctx.SaveChanges();
            }
        }

        public RelayCommand EditCommand
        {
            get => new RelayCommand(EditInvoice, s => InvoiceSel != null);
        }

        private void EditInvoice(object obj)
        {
            Debug.Assert(InvoiceSel != null);
            var invoice = InvoiceSel as VmInvoice;
            var modelView = new EditInvoiceViewModel(invoice);
            var f = new EditInvoiceView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                InvoiceSel.Status = (int)modelView.Status;
                InvoiceSel.Person = ctx.Person.Find(modelView.PersonId);
                InvoiceSel.Sm = modelView.Sm;
                ctx.InvoiceDetail.RemoveRange(InvoiceSel.InvoiceDetail);
                InvoiceSel.InvoiceDetail.Clear();
                foreach (var o in modelView.ListInvoiceDetail)
                {
                    InvoiceSel.InvoiceDetail.Add(o.Obj);
                }
                ctx.SaveChanges();
            }
        }

        public RelayCommand DelCommand
        {
            get => new RelayCommand(DelInvoice, s => InvoiceSel != null);
        }

        private void DelInvoice(object obj)
        {
            Debug.Assert(InvoiceSel != null);
            var invoice = InvoiceSel as VmInvoice;
            ctx.InvoiceDetail.RemoveRange(InvoiceSel.InvoiceDetail);
            ctx.Invoice.Remove(InvoiceSel.Obj);
            ctx.SaveChanges();
            InvoiceList.Remove(InvoiceSel);
        }

        public RelayCommand PrintCommand
        {
            get => new RelayCommand(PrnInvoice, s => InvoiceSel != null);
        }

        private void PrnInvoice(object obj)
        {
            Debug.Assert(InvoiceSel != null);
            PrintInvoice.DoIt(InvoiceSel.Obj);
        }

        public RelayCommand PayCommand
        {
            get => new RelayCommand(PayInvoice, s => InvoiceSel != null);
        }

        private void PayInvoice(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

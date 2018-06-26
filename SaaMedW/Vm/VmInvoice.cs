using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmInvoice : ViewModelBase
    {
        private Invoice m_object;

        public VmInvoice()
        {
            m_object = new Invoice();
        }
        public VmInvoice(Invoice obj)
        {
            m_object = obj;
        }
        public Invoice Obj
        {
            get => m_object;
        }
        public int Id
        {
            get => m_object.Id;
            set
            {
                m_object.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public DateTime Dt
        {
            get => m_object.Dt;
            set
            {
                m_object.Dt = value;
                OnPropertyChanged("Dt");
            }
        }
        public enumStatusInvoice Status
        {
            get => m_object.Status;
            set
            {
                m_object.Status = value;
                OnPropertyChanged("Status");
            }
        }
        public int PersonId
        {
            get => m_object.PersonId;
            set
            {
                m_object.PersonId = value;
                OnPropertyChanged("PersonId");
            }
        }
        public decimal Sm
        {
            get => m_object.Sm;
            set
            {
                m_object.Sm = value;
                OnPropertyChanged("Sm");
            }
        }
        public decimal Payed
        {
            get => m_object.Payed;
            set
            {
                m_object.Payed = value;
                OnPropertyChanged("Payed");
            }
        }
        public Person Person
        {
            get => m_object.Person;
            set
            {
                m_object.Person = value;
                OnPropertyChanged("Person");
            }
        }
        public ICollection<InvoiceDetail> InvoiceDetail
        {
            get => m_object.InvoiceDetail;
            set
            {
                m_object.InvoiceDetail = value;
                OnPropertyChanged("InvoiceDetail");
            }
        }
    }
}

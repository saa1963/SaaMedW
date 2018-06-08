using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmInvoiceDetail : ViewModelBase
    {
        private InvoiceDetail m_object;

        public VmInvoiceDetail()
        {
            m_object = new InvoiceDetail();
        }
        public VmInvoiceDetail(InvoiceDetail obj)
        {
            m_object = obj;
        }
        public InvoiceDetail Obj
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
        public string BenefitName
        {
            get => m_object.BenefitName;
            set
            {
                m_object.BenefitName = value;
                OnPropertyChanged("BenefitName");
            }
        }
        public int Kol
        {
            get => m_object.Kol;
            set
            {
                m_object.Kol = value;
                Sm = value * Price;
                OnPropertyChanged("Kol");
            }
        }
        public decimal Price
        {
            get => m_object.Price;
            set
            {
                m_object.Price = value;
                Sm = value * Kol;
                OnPropertyChanged("Price");
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
        public int InvoiceId
        {
            get => m_object.InvoiceId;
            set
            {
                m_object.InvoiceId = value;
                OnPropertyChanged("InvoiceId");
            }
        }
        public Invoice Invoice
        {
            get => m_object.Invoice;
            set
            {
                m_object.Invoice = value;
                OnPropertyChanged("Invoice");
            }
        }
    }
}

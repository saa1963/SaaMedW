using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class PayInvoiceViewModel: NotifyPropertyChanged, IDataErrorInfo
    {
        public PayInvoiceViewModel()
        {
            var temp
                = (enumPaymentType[])Enum.GetValues(typeof(enumPaymentType));
            PaymentTypeList = new List<enumPaymentType>();
            foreach(var o in temp)
            {
                if (o != enumPaymentType.Возврат)
                    PaymentTypeList.Add(o);
            }
        }

        public List<enumPaymentType> PaymentTypeList { get; set; }

        private enumPaymentType m_PaymentType;
        public enumPaymentType PaymentType
        {
            get => m_PaymentType;
            set
            {
                m_PaymentType = value;
                OnPropertyChanged("PaymentType");
            }
        }
        private decimal m_КОплате;
        public decimal КОплате
        {
            get => m_КОплате;
            set
            {
                m_КОплате = value;
                OnPropertyChanged("КОплате");
            }
        }
        private decimal m_Sm;
        public decimal Sm
        {
            get => m_Sm;
            set
            {
                m_Sm = value;
                Сдача = Math.Max(Sm - КОплате, 0);
                OnPropertyChanged("Sm");
            }
        }
        private decimal m_Сдача;
        public decimal Сдача
        {
            get => m_Сдача;
            set
            {
                m_Сдача = value;
                OnPropertyChanged("Сдача");
            }
        }
        private string m_Email;
        public string Email
        {
            get
            {
                if (String.IsNullOrWhiteSpace(m_Email))
                    return null;
                else
                    return m_Email;
            }
            set
            {
                m_Email = value;
                OnPropertyChanged("Email");
            }
        }
        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Sm":
                        if (Sm <= 0)
                            result = "Не введена сумма.";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error { get; set; } = String.Empty;
    }
}

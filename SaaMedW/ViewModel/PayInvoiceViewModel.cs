using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class PayInvoiceViewModel: ViewModelBase, IDataErrorInfo
    {
        private decimal m_Sm;
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

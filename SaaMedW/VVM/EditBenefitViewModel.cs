using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW
{
    public class EditBenefitViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
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
        private decimal m_Price;
        public decimal Price
        {
            get => m_Price;
            set
            {
                m_Price = value;
                OnPropertyChanged("Price");
            }
        }
        private int m_Duration;
        public int Duration
        {
            get => m_Duration;
            set
            {
                m_Duration = value;
                OnPropertyChanged("Duration");
            }
        }
        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrWhiteSpace(Name))
                            result = "Не введено наименование";
                        break;
                    case "Duration":
                        if (Duration <= 0)
                            result = "Не введена продолжительность";
                        break;
                    case "Price":
                        if (Price <= 0)
                            result = "Не введена цена";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error => "";
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    class EditStringViewModel: NotifyPropertyChanged, IDataErrorInfo
    {
        private string m_Name;
        private string m_Header = "Наименование";

        public EditStringViewModel() { }
        public EditStringViewModel(VmSpecialty specialty)
        {
            m_Name = specialty.Name;
        }

        public string Name
        {
            get => m_Name;
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Header
        {
            get => m_Header;
            set
            {
                m_Header = value;
                OnPropertyChanged("Header");
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
                            result = "Не введено наименование.";
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

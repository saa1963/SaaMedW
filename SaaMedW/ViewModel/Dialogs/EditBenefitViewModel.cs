using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW.ViewModel
{
    public class EditBenefitViewModel : VmBenefit, IDataErrorInfo
    {
        public EditBenefitViewModel():base()
        {
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

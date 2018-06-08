using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW
{
    public class InvoiceStatusToIntConverter : IValueConverter
    {
        Array ar = Enum.GetValues(typeof(enStatusInvoice));
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int)value;
            return ar.GetValue(v);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            enStatusInvoice v = (enStatusInvoice)value;
            for (int i = 0; i < ar.Length; i++)
            {
                if ((enStatusInvoice)ar.GetValue(i) == v)
                    return i;
            }
            return null;
        }
    }
}

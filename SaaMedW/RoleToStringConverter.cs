using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SaaMedW
{
    public class RoleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = (int)value;
            Debug.Assert(v == 0 || v == 1);
            if (v == 0) return Role.Администратор;
            else return Role.Пользователь;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Role r = (Role)value;
            Debug.Assert(r == Role.Администратор || r == Role.Пользователь);
            if (r == Role.Администратор) return 0;
            else return 1;
        }
    }
}

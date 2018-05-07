using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SaaMedW
{
    public class SpecialtyListRule : ValidationRule
    {
        public SpecialtyListRule()
        {
        }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int id;
            try
            {
                if (((string)value).Length > 0)
                    id = Int32.Parse((String)value);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, "Фигня полная " + e.Message);
            }
            return ValidationResult.ValidResult;
        }
    }
}

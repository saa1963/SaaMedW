using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public partial class Options
    {
        public static string GetString(enumParameterType type, int userId)
        {
            using (var ctx = new SaaMedEntities())
            {
                var opt = ctx.Options.Find(new object[] { type, userId });
                Debug.Assert(opt != null);
                Debug.Assert(opt.DataType == "System.String");
                return opt.ParameterValue;
            }
        }
        public object GetObject()
        {
            switch(DataType)
            {
                case "System.String":
                    return ParameterValue;
                case "System.IO.Path":
                    return ParameterValue;
                case "System.Int32":
                    if (Int32.TryParse(ParameterValue, out int resultInt))
                        return resultInt;
                    else
                    {
                        Debug.Assert(false, "Недопустимое значение параметра.");
                        return null;
                    }
                case "System.Decimal":
                    if (Decimal.TryParse(ParameterValue, out decimal resultDecimal))
                        return resultDecimal;
                    else
                    {
                        Debug.Assert(false, "Недопустимое значение параметра.");
                        return null;
                    }
                case "System.DateTime":
                    var f = new CultureInfo("ru-RU").DateTimeFormat;
                    if (DateTime.TryParse(ParameterValue, f, DateTimeStyles.None, out DateTime resultDateTime))
                        return resultDateTime;
                    else
                    {
                        Debug.Assert(false, "Недопустимое значение параметра.");
                        return null;
                    }
                default:
                    Debug.Assert(false, "Недопустимый тип параметра.");
                    return null;
            }
        }
        public void SetObject(object Value)
        {
            if (Value is string)
                ParameterValue = Value.ToString();
            else if (Value is System.IO.Path)
                ParameterValue = Value.ToString();
            else if (Value is int)
                ParameterValue = ((int)Value).ToString();
            else if (Value is decimal)
                ParameterValue = ((decimal)Value).ToString();
            else if (Value is DateTime)
            {
                ParameterValue = ((DateTime)Value).ToString(new CultureInfo("ru-RU").DateTimeFormat);
            }
            else
                Debug.Assert(false, "Недопустимый тип параметра.");
        }
    }
}

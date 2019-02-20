using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class OptionType
    {
        public Type type { get; set; }
        public enumProfile profile { get; set; }
        public object defaultValue { get; set; }
    }
    public partial class Options
    {
        public static Dictionary<enumParameterType, OptionType> ВсеВидыПараметров { get; set; } =
            new Dictionary<enumParameterType, OptionType>()
            {
                { enumParameterType.Наименование_организации,
                    new OptionType()
                    {
                        type = typeof(string), profile = enumProfile.Общий, defaultValue = ""
                    }
                },
                {enumParameterType.Коэффициент_для_Excel,
                    new OptionType()
                    { type = typeof(int), profile = enumProfile.ЛокальныйВсеПользователи, defaultValue = 88 }
                },
                {enumParameterType.Настройки_ФР,
                    new OptionType()
                    { type = typeof(string), profile = enumProfile.ЛокальныйВсеПользователи, defaultValue = "" }
                }
            };
        public string Name
        {
            get => Enum.GetName(typeof(enumParameterType), ParameterType).Replace('_', ' ');
        }
        /// <summary>
        /// Возвращает из БД указанный параметр или значение по умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">Тип параметра</param>
        /// <returns></returns>
        public static T GetParameter<T>(enumParameterType type)
        {
            string compId = "0";
            int userId = 0;
            switch (ВсеВидыПараметров[type].profile)
            {
                case enumProfile.Общий:
                    compId = "0";
                    userId = 0;
                    break;
                case enumProfile.ЛокальныйПользователя:
                    compId = Global.Source.GetMotherboardId();
                    userId = Global.Source.RUser.Id;
                    break;
                case enumProfile.ЛокальныйВсеПользователи:
                    compId = Global.Source.GetMotherboardId();
                    userId = 0;
                    break;
                case enumProfile.ПеремещаемыйПользователя:
                    compId = "0";
                    userId = Global.Source.RUser.Id;
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            using (var ctx = new SaaMedEntities())
            {

                var opt = ctx.Options.Find(new object[] { type, userId, compId });
                if (opt != null)
                    return opt.GetObject<T>();
                else
                    return (T)Convert.ChangeType(ВсеВидыПараметров[type].defaultValue, typeof(T));
            }
        }

        /// <summary>
        /// Возвращает значение параметра
        /// </summary>
        /// <returns></returns>
        public object GetObject()
        {
            Type type = ВсеВидыПараметров[this.ParameterType].type;
            if (type == typeof(string))
                return ParameterValue;
            else if (type == typeof(System.IO.Path))
                return ParameterValue;
            else if (type == typeof(int))
                if (Int32.TryParse(ParameterValue, out int resultInt))
                    return resultInt;
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return null;
                }
            else if (type == typeof(decimal))
                if (Decimal.TryParse(ParameterValue, out decimal resultDecimal))
                    return resultDecimal;
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return null;
                }
            else if (type == typeof(DateTime))
            {
                var f = new CultureInfo("ru-RU").DateTimeFormat;
                if (DateTime.TryParse(ParameterValue, f, DateTimeStyles.None, out DateTime resultDateTime))
                    return resultDateTime;
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return null;
                }
            }
            else
            {
                Debug.Assert(false, "Недопустимый тип параметра.");
                return null;
            }
        }
        public T GetObject<T>()
        {
            Type type = ВсеВидыПараметров[this.ParameterType].type;
            if (type == typeof(string))
                return (T)Convert.ChangeType(ParameterValue, typeof(T));
            else if (type == typeof(System.IO.Path))
                return (T)Convert.ChangeType(ParameterValue, typeof(T));
            else if (type == typeof(int))
                if (Int32.TryParse(ParameterValue, out int resultInt))
                    return (T)Convert.ChangeType(resultInt, typeof(T));
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return default(T);
                }
            else if (type == typeof(decimal))
                if (Decimal.TryParse(ParameterValue, out decimal resultDecimal))
                    return (T)Convert.ChangeType(resultDecimal, typeof(T));
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return default(T);
                }
            else if (type == typeof(DateTime))
            {
                var f = new CultureInfo("ru-RU").DateTimeFormat;
                if (DateTime.TryParse(ParameterValue, f, DateTimeStyles.None, out DateTime resultDateTime))
                    return (T)Convert.ChangeType(resultDateTime, typeof(T));
                else
                {
                    Debug.Assert(false, "Недопустимое значение параметра.");
                    return default(T);
                }
            }
            else
            {
                Debug.Assert(false, "Недопустимый тип параметра.");
                return default(T);
            }
        }
        public void SetObject(object Value)
        {
            object dv = ВсеВидыПараметров[this.ParameterType].defaultValue;

            if (Value == null)
                dv = ВсеВидыПараметров[this.ParameterType].defaultValue;
            else
                dv = Value;
            if (Value is string)
                ParameterValue = dv.ToString();
            else if (Value is System.IO.Path)
                ParameterValue = dv.ToString();
            else if (Value is int)
                ParameterValue = ((int)dv).ToString();
            else if (Value is decimal)
                ParameterValue = ((decimal)dv).ToString();
            else if (Value is DateTime)
            {
                ParameterValue = ((DateTime)dv).ToString(new CultureInfo("ru-RU").DateTimeFormat);
            }
            else
            {
                Debug.Assert(false, "Недопустимый тип параметра.");
            }
        }
    }
}

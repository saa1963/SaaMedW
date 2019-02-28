using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public enum Role { Администратор = 0, Пользователь = 1 };
    public enum enTaxSystem
    {
        Общая = 0,
        Упрощенная_Доход = 1,
        Упрощенная_Доход_Минус_Расход = 2,
        ЕНВД = 3,
        Единый_сельскохозяйственный_налог = 4,
        Патентная = 5
    }
    public enum enNds
    {
        Нет_НДС = 0,
        Процент_0 = 1,
        Процент_10 = 2,
        Процент_20 = 3
    }
}

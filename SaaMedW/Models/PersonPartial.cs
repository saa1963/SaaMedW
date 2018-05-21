using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public partial class Person
    {
        public string Fio
        {
            get => LastName + " " + FirstName + (MiddleName != null ? " " : "") + MiddleName ?? "";
        }
        public string FioBirthday
        {
            get => (Fio + " " + (BirthDate.HasValue ? BirthDate.Value.ToString("dd.MM.yyyy") : "")).TrimEnd();
        }
    }
}

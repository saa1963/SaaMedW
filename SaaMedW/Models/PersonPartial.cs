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
        public string FullAddress
        {
            get
            {
                var mas = new List<string>();
                if (!String.IsNullOrWhiteSpace(AddressSubject)) mas.Add(AddressSubject);
                if (!String.IsNullOrWhiteSpace(AddressRaion)) mas.Add(AddressRaion);
                if (!String.IsNullOrWhiteSpace(AddressCity)) mas.Add(AddressCity);
                if (!String.IsNullOrWhiteSpace(AddressPunkt)) mas.Add(AddressPunkt);
                if (!String.IsNullOrWhiteSpace(AddressStreet)) mas.Add(AddressStreet);
                if (!String.IsNullOrWhiteSpace(AddressHouse)) mas.Add(AddressHouse);
                return string.Join(" ", mas);
            }
        }
    }
}

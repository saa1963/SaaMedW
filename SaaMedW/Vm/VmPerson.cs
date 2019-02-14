using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class VmPerson : NotifyPropertyChanged
    {
        private Person m_object;

        public VmPerson()
        {
            m_object = new Person();
            m_object.CreateDate = DateTime.Now;
        }
        public VmPerson(Person par)
        {
            m_object = par;
        }

        public Person Obj
        {
            get => m_object;
            set { m_object = value; OnPropertyChanged("Obj"); }
        }
        public int Id
        {
            get => m_object.Id;
            set
            {
                m_object.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string LastName
        {
            get => m_object.LastName;
            set
            {
                m_object.LastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string FirstName
        {
            get => m_object.FirstName;
            set
            {
                m_object.FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string MiddleName
        {
            get => m_object.MiddleName;
            set
            {
                m_object.MiddleName = value;
                OnPropertyChanged("MiddleName");
            }
        }
        public DateTime? BirthDate
        {
            get => m_object.BirthDate;
            set
            {
                m_object.BirthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        public string Phone
        {
            get => m_object.Phone;
            set
            {
                m_object.Phone = value;
                OnPropertyChanged("Phone");
            }
        }
        public int? Sex
        {
            get => m_object.Sex;
            set
            {
                m_object.Sex = value;
                OnPropertyChanged("Sex");
            }
        }
        public string Inn
        {
            get => m_object.Inn;
            set
            {
                m_object.Inn = value;
                OnPropertyChanged("Inn");
            }
        }
        public string Snils
        {
            get => m_object.Snils;
            set
            {
                m_object.Snils = value;
                OnPropertyChanged("Snils");
            }
        }
        public int? DocumentTypeId
        {
            get => m_object.DocumentTypeId;
            set
            {
                m_object.DocumentTypeId = value;
                OnPropertyChanged("DocumentTypeId");
            }
        }
        public string DocSeria
        {
            get => m_object.DocSeria;
            set
            {
                m_object.DocSeria = value;
                OnPropertyChanged("DocSeria");
            }
        }
        public string DocNumber
        {
            get => m_object.DocNumber;
            set
            {
                m_object.DocNumber = value;
                OnPropertyChanged("DocNumber");
            }
        }
        public string AddressSubject
        {
            get => m_object.AddressSubject;
            set
            {
                m_object.AddressSubject = value;
                OnPropertyChanged("AddressSubject");
            }
        }
        public string AddressRaion
        {
            get => m_object.AddressRaion;
            set
            {
                m_object.AddressRaion = value;
                OnPropertyChanged("AddressRaion");
            }
        }
        public string AddressCity
        {
            get => m_object.AddressCity;
            set
            {
                m_object.AddressCity = value;
                OnPropertyChanged("AddressCity");
            }
        }
        public string AddressPunkt
        {
            get => m_object.AddressPunkt;
            set
            {
                m_object.AddressPunkt = value;
                OnPropertyChanged("AddressPunkt");
            }
        }
        public string AddressStreet
        {
            get => m_object.AddressStreet;
            set
            {
                m_object.AddressStreet = value;
                OnPropertyChanged("AddressStreet");
            }
        }
        public string AddressHouse
        {
            get => m_object.AddressHouse;
            set
            {
                m_object.AddressHouse = value;
                OnPropertyChanged("AddressHouse");
            }
        }
        public string AddressFlat
        {
            get => m_object.AddressFlat;
            set
            {
                m_object.AddressFlat = value;
                OnPropertyChanged("AddressFlat");
            }
        }
        public int? Mestnost
        {
            get => m_object.Mestnost;
            set
            {
                m_object.Mestnost = value;
                OnPropertyChanged("Mestnost");
            }
        }
        public DateTime CreateDate
        {
            get => m_object.CreateDate;
            set
            {
                m_object.CreateDate = value;
                OnPropertyChanged("CreateDate");
            }
        }
        public string FullName
        {
            get => ToString();
        }
        public override string ToString()
        {
            return (LastName + " " + FirstName + " " + MiddleName ?? "").TrimEnd();
        }
    }
}

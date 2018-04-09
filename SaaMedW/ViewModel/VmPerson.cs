using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmPerson : INotifyPropertyChanged
    {
        private Person m_object;
        public event PropertyChangedEventHandler PropertyChanged;
        public VmPerson()
        {
            m_object = new Person();
        }
        public VmPerson(Person par)
        {
            m_object = par;
        }
        public Person Obj
        {
            get => m_object;
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
        public Nullable<int> Sex { get; set; }
        public string Inn { get; set; }
        public string Snils { get; set; }
        public Nullable<int> DocumentTypeId { get; set; }
        public string DocSeria { get; set; }
        public string DocNumber { get; set; }
        public string AddressSubject { get; set; }
        public string AddressRaion { get; set; }
        public string AddressCity { get; set; }
        public string AddressPunkt { get; set; }
        public string AddressStreet { get; set; }
        public string AddressHouse { get; set; }
        public string AddressFlat { get; set; }
        public Nullable<int> Mestnost { get; set; }
        private void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}

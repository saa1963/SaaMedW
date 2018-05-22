using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW.ViewModel
{
    public class EditPersonViewModel : ViewModelBase, IDataErrorInfo
    {
        private Person m_object;
        public List<IdName> SexList { get; } =
            new List<IdName> {
                new IdName { Id = 1, Name = "Мужской" },
                new IdName { Id = 2, Name = "Женский"} };
        public List<IdName> MestnostList { get; } =
            new List<IdName> {
                new IdName { Id = 1, Name = "Городская" },
                new IdName { Id = 2, Name = "Сельская"} };
        public ObservableCollection<VmDocumentType> DocTypeList { get; set; } =
            new ObservableCollection<VmDocumentType>();
        private SaaMedEntities ctx = new SaaMedEntities();

        public EditPersonViewModel()
        {
            m_object = new Person();
            FillDocType();
            m_object.CreateDate = DateTime.Now;
        }
        public EditPersonViewModel(Person par)
        {
            m_object = par;
            FillDocType();
        }
        public EditPersonViewModel(EditPersonViewModel obj)
        {
            m_object = new Person();
            Id = obj.Id;
            CopyProperties(obj);
        }
        public EditPersonViewModel Copy(EditPersonViewModel obj)
        {
            CopyProperties(obj);
            return this;
        }
        private void CopyProperties(EditPersonViewModel obj)
        {
            AddressCity = obj.AddressCity;
            AddressFlat = obj.AddressFlat;
            AddressHouse = obj.AddressHouse;
            AddressPunkt = obj.AddressPunkt;
            AddressRaion = obj.AddressRaion;
            AddressStreet = obj.AddressStreet;
            AddressSubject = obj.AddressSubject;
            BirthDate = obj.BirthDate;
            CreateDate = obj.CreateDate;
            DocNumber = obj.DocNumber;
            DocSeria = obj.DocSeria;
            DocumentTypeId = obj.DocumentTypeId;
            FirstName = obj.FirstName;
            Inn = obj.Inn;
            LastName = obj.LastName;
            Mestnost = obj.Mestnost;
            MiddleName = obj.MiddleName;
            Phone = obj.Phone;
            Sex = obj.Sex;
            Snils = obj.Snils;
        }
        private void FillDocType()
        {
            foreach(DocumentType o in ctx.DocumentType)
            {
                DocTypeList.Add(new VmDocumentType(o));
            }
        }
        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "LastName":
                        if (String.IsNullOrWhiteSpace(LastName))
                            result = "Не введена фамилия.";
                        break;
                    case "FirstName":
                        if (String.IsNullOrWhiteSpace(FirstName))
                            result = "Не введено имя.";
                        break;
                    default:
                        break;
                }
                return result;
            }
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
        public string Error => "";
    }
}

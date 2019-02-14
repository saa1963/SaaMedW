using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW
{
    public class EditPersonViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        public enSex[] SexList { get; } = (enSex[])Enum.GetValues(typeof(enSex));
        public enMestnost[] MestnostList { get; } = (enMestnost[])Enum.GetValues(typeof(enMestnost));
        public ObservableCollection<VmDocumentType> DocTypeList { get; set; } =
            new ObservableCollection<VmDocumentType>();
        private SaaMedEntities ctx = new SaaMedEntities();

        public EditPersonViewModel()
        {
            FillDocType();
        }
        public EditPersonViewModel(Person par)
        {
            CopyProperties(par);
            FillDocType();
        }
        private void CopyProperties(Person obj)
        {
            m_AddressCity = obj.AddressCity;
            m_AddressFlat = obj.AddressFlat;
            m_AddressHouse = obj.AddressHouse;
            m_AddressPunkt = obj.AddressPunkt;
            m_AddressRaion = obj.AddressRaion;
            m_AddressStreet = obj.AddressStreet;
            m_AddressSubject = obj.AddressSubject;
            m_BirthDate = obj.BirthDate;
            m_DocNumber = obj.DocNumber;
            m_DocSeria = obj.DocSeria;
            m_DocumentTypeId = obj.DocumentTypeId;
            m_FirstName = obj.FirstName;
            m_Inn = obj.Inn;
            m_LastName = obj.LastName;
            m_Mestnost = obj.Mestnost;
            m_MiddleName = obj.MiddleName;
            m_Phone = obj.Phone;
            m_Sex = obj.Sex;
            m_Snils = obj.Snils;
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

        //public Person Obj
        //{
        //    get => m_object;
        //}
        private int m_Id;
        public int Id
        {
            get => m_Id;
            set
            {
                m_Id = value;
                OnPropertyChanged("Id");
            }
        }
        private string m_LastName;
        public string LastName
        {
            get => m_LastName;
            set
            {
                m_LastName = value;
                OnPropertyChanged("LastName");
            }
        }
        private string m_FirstName;
        public string FirstName
        {
            get => m_FirstName;
            set
            {
                m_FirstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        private string m_MiddleName;
        public string MiddleName
        {
            get => m_MiddleName;
            set
            {
                m_MiddleName = value;
                OnPropertyChanged("MiddleName");
            }
        }
        private DateTime? m_BirthDate;
        public DateTime? BirthDate
        {
            get => m_BirthDate;
            set
            {
                m_BirthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        private string m_Phone;
        public string Phone
        {
            get => m_Phone;
            set
            {
                m_Phone = value;
                OnPropertyChanged("Phone");
            }
        }
        private enSex? m_Sex;
        public enSex? Sex
        {
            get => m_Sex;
            set
            {
                m_Sex = value;
                OnPropertyChanged("Sex");
            }
        }
        private string m_Inn;
        public string Inn
        {
            get => m_Inn;
            set
            {
                m_Inn = value;
                OnPropertyChanged("Inn");
            }
        }
        private string m_Snils;
        public string Snils
        {
            get => m_Snils;
            set
            {
                m_Snils = value;
                OnPropertyChanged("Snils");
            }
        }
        private int? m_DocumentTypeId;
        public int? DocumentTypeId
        {
            get => m_DocumentTypeId;
            set
            {
                m_DocumentTypeId = value;
                OnPropertyChanged("DocumentTypeId");
            }
        }
        private DocumentType m_DocumentType;
        public DocumentType DocumentType
        {
            get => m_DocumentType;
            set
            {
                m_DocumentType = value;
                OnPropertyChanged("DocumentType");
            }
        }
        private string m_DocSeria;
        public string DocSeria
        {
            get => m_DocSeria;
            set
            {
                m_DocSeria = value;
                OnPropertyChanged("DocSeria");
            }
        }
        private string m_DocNumber;
        public string DocNumber
        {
            get => m_DocNumber;
            set
            {
                m_DocNumber = value;
                OnPropertyChanged("DocNumber");
            }
        }
        private string m_AddressSubject;
        public string AddressSubject
        {
            get => m_AddressSubject;
            set
            {
                m_AddressSubject = value;
                OnPropertyChanged("AddressSubject");
            }
        }
        private string m_AddressRaion;
        public string AddressRaion
        {
            get => m_AddressRaion;
            set
            {
                m_AddressRaion = value;
                OnPropertyChanged("AddressRaion");
            }
        }
        private string m_AddressCity;
        public string AddressCity
        {
            get => m_AddressCity;
            set
            {
                m_AddressCity = value;
                OnPropertyChanged("AddressCity");
            }
        }
        private string m_AddressPunkt;
        public string AddressPunkt
        {
            get => m_AddressPunkt;
            set
            {
                m_AddressPunkt = value;
                OnPropertyChanged("AddressPunkt");
            }
        }
        private string m_AddressStreet;
        public string AddressStreet
        {
            get => m_AddressStreet;
            set
            {
                m_AddressStreet = value;
                OnPropertyChanged("AddressStreet");
            }
        }
        private string m_AddressHouse;
        public string AddressHouse
        {
            get => m_AddressHouse;
            set
            {
                m_AddressHouse = value;
                OnPropertyChanged("AddressHouse");
            }
        }
        private string m_AddressFlat;
        public string AddressFlat
        {
            get => m_AddressFlat;
            set
            {
                m_AddressFlat = value;
                OnPropertyChanged("AddressFlat");
            }
        }
        public enMestnost? m_Mestnost;
        public enMestnost? Mestnost
        {
            get => m_Mestnost;
            set
            {
                m_Mestnost = value;
                OnPropertyChanged("Mestnost");
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

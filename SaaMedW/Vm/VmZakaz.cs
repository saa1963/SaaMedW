using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    class VmZakaz: NotifyPropertyChanged
    {
        protected Zakaz m_object;

        public VmZakaz()
        {
            m_object = new Zakaz();
        }
        public VmZakaz(Zakaz obj)
        {
            m_object = obj;
        }
        public Zakaz Obj
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
        public DateTime Dt
        {
            get => m_object.Dt;
            set
            {
                m_object.Dt = value;
                OnPropertyChanged("Dt");
            }
        }
        public int Num
        {
            get => m_object.Num;
            set
            {
                m_object.Num = value;
                OnPropertyChanged("Num");
            }
        }
        public int PersonId
        {
            get => m_object.PersonId;
            set
            {
                m_object.PersonId = value;
                OnPropertyChanged("PersonId");
            }
        }
        public Person Person
        {
            get => m_object.Person;
            set
            {
                m_object.Person = value;
                OnPropertyChanged("Person");
            }
        }
        public bool Dms
        {
            get => m_object.Dms;
            set
            {
                m_object.Dms = value;
                OnPropertyChanged("Dms");
            }
        }
        public string Polis
        {
            get => m_object.Polis;
            set
            {
                m_object.Polis = value;
                OnPropertyChanged("Polis");
            }
        }
        public int? DmsCompanyId
        {
            get => m_object.DmsCompanyId;
            set
            {
                m_object.DmsCompanyId = value;
                OnPropertyChanged("DmsCompanyId");
            }
        }
        public DmsCompany DmsCompany
        {
            get => m_object.DmsCompany;
            set
            {
                m_object.DmsCompany = value;
                OnPropertyChanged("DmsCompany");
            }
        }
        public ICollection<Zakaz1> Zakaz1
        {
            get => m_object.Zakaz1;
            set
            {
                m_object.Zakaz1 = value;
                OnPropertyChanged("Zakaz1");
            }
        }
    }
}

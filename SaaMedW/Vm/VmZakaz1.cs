using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    class VmZakaz1: NotifyPropertyChanged
    {
        protected Zakaz1 m_object;

        public VmZakaz1()
        {
            m_object = new Zakaz1();
        }
        public VmZakaz1(Zakaz1 obj)
        {
            m_object = obj;
        }
        public Zakaz1 Obj
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
        public int? BenefitId
        {
            get => m_object.BenefitId;
            set
            {
                m_object.BenefitId = value;
                OnPropertyChanged("BenefitId");
            }
        }
        public int PersonalId
        {
            get => m_object.PersonalId;
            set
            {
                m_object.PersonalId = value;
                OnPropertyChanged("PersonalId");
            }
        }
        public decimal Price
        {
            get => m_object.Price;
            set
            {
                m_object.Price = value;
                OnPropertyChanged("Price");
            }
        }
        public int Kol
        {
            get => m_object.Kol;
            set
            {
                m_object.Kol = value;
                OnPropertyChanged("Kol");
            }
        }
        public int ZakazId
        {
            get => m_object.ZakazId;
            set
            {
                m_object.ZakazId = value;
                OnPropertyChanged("ZakazId");
            }
        }
        public string BenefitName
        {
            get => m_object.BenefitName;
            set
            {
                m_object.BenefitName = value;
                OnPropertyChanged("BenefitName");
            }
        }
        public Benefit Benefit
        {
            get => m_object.Benefit;
            set
            {
                m_object.Benefit = value;
                OnPropertyChanged("Benefit");
            }
        }
        public Personal Personal
        {
            get => m_object.Personal;
            set
            {
                m_object.Personal = value;
                OnPropertyChanged("Personal");
            }
        }
        public Zakaz Zakaz
        {
            get => m_object.Zakaz;
            set
            {
                m_object.Zakaz = value;
                OnPropertyChanged("Zakaz");
            }
        }
    }
}

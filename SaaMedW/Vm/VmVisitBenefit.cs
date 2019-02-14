using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class VmVisitBenefit : NotifyPropertyChanged
    {
        private VisitBenefit m_object;

        public VmVisitBenefit()
        {
            m_object = new VisitBenefit();
        }
        public VmVisitBenefit(VisitBenefit obj)
        {
            m_object = obj;
        }
        public VisitBenefit Obj
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
        public int VisitId
        {
            get => m_object.VisitId;
            set
            {
                m_object.VisitId = value;
                OnPropertyChanged("VisitId");
            }
        }
        public int BenefitId
        {
            get => m_object.BenefitId;
            set
            {
                m_object.BenefitId = value;
                OnPropertyChanged("BenefitId");
            }
        }
        public int Status
        {
            get => m_object.Status;
            set
            {
                m_object.Status = value;
                OnPropertyChanged("Status");
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
        public Benefit Benefit
        {
            get => m_object.Benefit;
            set
            {
                m_object.Benefit = value;
                OnPropertyChanged("Benefit");
            }
        }
        public Visit Visit
        {
            get => m_object.Visit;
            set
            {
                m_object.Visit = value;
                OnPropertyChanged("Visit");
            }
        }
    }
}

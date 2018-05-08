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
    public class VmPersonal : ViewModelBase
    {
        private Personal m_object;

        public VmPersonal()
        {
            m_object = new Personal();
        }
        public VmPersonal(Personal par)
        {
            m_object = par;
        }
        public Personal Obj
        {
            get => m_object;
            set
            {
                m_object = value;
                OnPropertyChanged("Obj");
            }
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
        public string Fio
        {
            get => m_object.Fio;
            set
            {
                m_object.Fio = value;
                OnPropertyChanged("Fio");
            }
        }
        public bool Active
        {
            get => m_object.Active;
            set
            {
                m_object.Active = value;
                OnPropertyChanged("Active");
            }
        }
        public ICollection<PersonalSpecialty> PersonalSpecialty
        {
            get => m_object.PersonalSpecialty;
            set
            {
                m_object.PersonalSpecialty = value;
                OnPropertyChanged("PersonalSpecialty");
            }
        }
    }
}

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
        public int? Specialty
        {
            get => m_object.Specialty;
            set
            {
                m_object.Specialty = value;
                OnPropertyChanged("Specialty");
                OnPropertyChanged("SpecialtyName");
            }
        }
        public void SetSpecialty1(SaaMedEntities ctx)
        {
            ctx.Entry(m_object).Reference(s => s.Specialty1).Load();
            OnPropertyChanged("Specialty");
            OnPropertyChanged("SpecialtyName");
        }
        public string SpecialtyName
        {
            get => m_object.Specialty1?.Name;
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
    }
}

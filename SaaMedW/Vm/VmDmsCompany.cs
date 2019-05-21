using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    class VmDmsCompany: NotifyPropertyChanged
    {
        protected DmsCompany m_object;

        public VmDmsCompany()
        {
            m_object = new DmsCompany();
        }
        public VmDmsCompany(DmsCompany obj)
        {
            m_object = obj;
        }
        public DmsCompany Obj
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
        public string Name
        {
            get => m_object.Name;
            set
            {
                m_object.Name = value;
                OnPropertyChanged("Name");
            }
        }
    }
}

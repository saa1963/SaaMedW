using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmSpecialty : ViewModelBase, IDataErrorInfo
    {
        private Specialty m_object;
        public VmSpecialty()
        {
            m_object = new Specialty();
        }
        public VmSpecialty(Specialty obj)
        {
            m_object = obj;
        }
        public VmSpecialty(VmSpecialty obj)
        {
            m_object = new Specialty();
            m_object.Id = obj.Id;
            m_object.Name = obj.Name;
        }
        public VmSpecialty Copy(VmSpecialty obj)
        {
            this.Name = obj.Name;
            return this;
        }
        public Specialty Obj
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
        public string Name
        {
            get => m_object.Name;
            set
            {
                m_object.Name = value;
                OnPropertyChanged("Name");
            }
        }
        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (String.IsNullOrWhiteSpace(Name))
                            result = "Не введено наименование.";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error => "";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class VmMKB: NotifyPropertyChanged
    {
        private MKB m_object;
        public VmMKB()
        {
            m_object = new MKB();
        }
        public VmMKB(MKB _object)
        {
            m_object = _object;
        }
        public string Kod
        {
            get => m_object.Kod;
            set
            {
                m_object.Kod = value;
                OnPropertyChanged("Kod");
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
        public string Parent
        {
            get => m_object.Parent;
            set
            {
                m_object.Parent = value;
                OnPropertyChanged("Parent");
            }
        }
    }
}

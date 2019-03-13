using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class VmMKB: NotifyPropertyChanged
    {
        private MKB m_object;
        private ObservableCollection<VmMKB> m_ChildMkb = new ObservableCollection<VmMKB>();
        public VmMKB(List<MKB> _lst,  MKB _object)
        {
            m_object = _object;
            foreach (var o in _lst.Where(s => s.Parent == _object.Kod))
            {
                m_ChildMkb.Add(new VmMKB(_lst, o));
            }
        }
        public ObservableCollection<VmMKB> ChildMkb
        {
            get => m_ChildMkb;
            set
            {
                m_ChildMkb = value;
                OnPropertyChanged("ChildMkb");
            }
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
        public string DisplayName => m_object.Kod + " " + m_object.Name;
    }
}

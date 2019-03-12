using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class MkbViewModel: NotifyPropertyChanged
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private ObservableCollection<VmMKB> m_MkbList = new ObservableCollection<VmMKB>();
        public ObservableCollection<VmMKB> MkbList
        {
            get => m_MkbList;
            set
            {
                m_MkbList = value;
                OnPropertyChanged("MbkList");
            }
        }
        public MkbViewModel()
        {
            RefreshData();
        }

        private void RefreshData()
        {
            m_MkbList.Clear();
            var q = ctx.MKB.Where(s => s.Parent == null).OrderBy(s => s.Kod);
            foreach(var o in q)
            {
                m_MkbList.Add(new VmMKB(o));
            }
        }
    }
}

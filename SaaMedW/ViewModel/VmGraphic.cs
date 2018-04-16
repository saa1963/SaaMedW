using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmGraphic : ViewModelBase
    {
        private Graphic m_object;

        public int Id
        {
            get => m_object.Id;
            set
            {
                m_object.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public int PersonalId { get; set; }
        public System.DateTime Dt { get; set; }
        public int H1 { get; set; }
        public int M1 { get; set; }
        public int H2 { get; set; }
        public int M2 { get; set; }
    }
}

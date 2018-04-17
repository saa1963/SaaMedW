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

        public static VmGraphic GetGraphic(SaaMedEntities ctx, DateTime dt, int? pid)
        {
            VmGraphic o;
            if (pid.HasValue)
            {
                o = new VmGraphic(ctx.Graphic.FirstOrDefault(s => s.Dt == dt && s.PersonalId == pid.Value));
            }
            else
            {
                o = new VmGraphic();
                o.Dt = dt;
            }  
            return o;
        }
        public VmGraphic()
        {
            m_object = new Graphic();
        }
        public VmGraphic(Graphic obj)
        {
            m_object = obj;
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
        public int PersonalId
        {
            get => m_object.PersonalId;
            set
            {
                m_object.PersonalId = value;
                OnPropertyChanged("PersonalId");
            }
        }
        public System.DateTime Dt
        {
            get => m_object.Dt;
            set
            {
                m_object.Dt = value;
                OnPropertyChanged("Dt");
            }
        }
        public int H1
        {
            get => m_object.H1;
            set
            {
                m_object.H1 = value;
                OnPropertyChanged("H1");
            }
        }
        public int M1
        {
            get => m_object.M1;
            set
            {
                m_object.M1 = value;
                OnPropertyChanged("M1");
            }
        }
        public int H2
        {
            get => m_object.H2;
            set
            {
                m_object.H2 = value;
                OnPropertyChanged("H2");
            }
        }
        public int M2
        {
            get => m_object.M2;
            set
            {
                m_object.M2 = value;
                OnPropertyChanged("M2");
            }
        }
    }
}

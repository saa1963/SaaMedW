using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmGraphic : ViewModelBase
    {
        private Graphic m_object;

        public static ListGraphicViewModel GetGraphics(SaaMedEntities ctx, DateTime dt, int? pid)
        {
            var lst = new ListGraphicViewModel(ctx);
            IQueryable<Graphic> q;
            if (pid.HasValue)
                q = ctx.Graphic.Where(s => s.Dt == dt && s.PersonalId == pid.Value);
            else
                q = ctx.Graphic.Where(s => s.Dt == dt);
            foreach (var graphic in q)
                lst.Add(new VmGraphic(graphic));
            return lst;
        }
        public VmGraphic()
        {
            m_object = new Graphic();
        }
        public VmGraphic(Graphic obj)
        {
            m_object = obj;
        }
        public Graphic Obj
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
                OnPropertyChanged("Interval");
            }
        }
        public int M1
        {
            get => m_object.M1;
            set
            {
                m_object.M1 = value;
                OnPropertyChanged("M1");
                OnPropertyChanged("Interval");
            }
        }
        public int H2
        {
            get => m_object.H2;
            set
            {
                m_object.H2 = value;
                OnPropertyChanged("H2");
                OnPropertyChanged("Interval");
            }
        }
        public int M2
        {
            get => m_object.M2;
            set
            {
                m_object.M2 = value;
                OnPropertyChanged("M2");
                OnPropertyChanged("Interval");
            }
        }
        public Personal personal
        {
            get => m_object.Personal;
            set
            {
                m_object.Personal = value;
                OnPropertyChanged("personal");
            }
        }
        public string Interval
        {
            get
            {
                if (H1 != 0 && H2 != 0)
                    return H1.ToString() + ":" + M1.ToString("00") + "-" + H2.ToString() + ":" + M2.ToString("00");
                else
                    return "";
            }
        }
        //public string Text
        //{
        //    get
        //    {
        //        Dt.ToString("dd.MM.yy" + )
        //    }
        //}
    }
}

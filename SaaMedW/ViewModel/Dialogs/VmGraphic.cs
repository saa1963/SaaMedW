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

        public static ObservableCollection<VmGraphic> GetGraphics(SaaMedEntities ctx, DateTime dt, int? pid)
        {
            var lst = new ObservableCollection<VmGraphic>();
            IQueryable<Graphic> q;
            if (pid.HasValue)
                q = ctx.Graphic.Where(s => s.Dt == dt && s.PersonalId == pid.Value);
            else
                q = ctx.Graphic.Where(s => s.Dt == dt);
            foreach (var graphic in q)
                lst.Add(new VmGraphic(graphic));
            //if (lst.Count == 0)
            //{
            //    var o = new VmGraphic();
            //    o.Dt = dt;
            //    o.personal = ctx.Personal.Find(5);
            //    o.H1 = 8;
            //    o.M1 = 0;
            //    o.H2 = 12;
            //    o.M2 = 0;
            //    lst.Add(o);
            //    lst.Add(o);
            //}
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

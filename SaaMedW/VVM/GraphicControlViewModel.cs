using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW
{
    public class GraphicControlViewModel : ObservableCollection<VmGraphic>
    {
        private SaaMedEntities ctx;
        private ICollectionView view;
        public GraphicViewModel parent { get; set; }
        public GraphicControlViewModel(SaaMedEntities _ctx)
        {
            ctx = _ctx;
            view = CollectionViewSource.GetDefaultView(this);
        }
        public VmGraphic Current
        {
            get => view.CurrentItem as VmGraphic;
            set => view.MoveCurrentTo(value);
        }
        public DateTime Dt { get; set; }
        public int Ind { get; set; }
        public Personal CurrentSotr { get; set; }
        public double Opacity
        {
            get
            {
                if (Dt < DateTime.Today) return 0.2;
                else return 1;
            }
        }
        public RelayCommand AddSotrCommand
        {
            get { return new RelayCommand(AddSotrProc); }
        }
        private void AddSotrProc(object obj)
        {
            int ind = (int)obj;
            var mv = new EditGraphicViewModel();
            mv.SotrCurrent = this.CurrentSotr;
            var f = new EditGraphic() { DataContext = mv };
            if (f.ShowDialog() ?? false)
            {
                var o = new Graphic();
                o.Dt = Dt;
                o.H1 = mv.H1;
                o.M1 = mv.M1;
                o.H2 = mv.H2;
                o.M2 = mv.M2;
                o.PersonalId = mv.SotrCurrent.Id;
                o.Personal = ctx.Personal.Find(mv.SotrCurrent.Id);
                ctx.Graphic.Add(o);
                ctx.SaveChanges();
                this.Add(new VmGraphic(o));
            }
        }

        public RelayCommand EditSotrCommand
        {
            get { return new RelayCommand(EditSotrProc); }
        }
        private void EditSotrProc(object obj)
        {
            VmGraphic o = Current;
            if (o == null) return;
            var mv = new EditGraphicViewModel();
            mv.SotrCurrent = o.personal;
            mv.H1 = o.H1;
            mv.M1 = o.M1;
            mv.H2 = o.H2;
            mv.M2 = o.M2;
            var f = new EditGraphic() { DataContext = mv };
            if (f.ShowDialog() ?? false)
            {
                o.H1 = mv.H1;
                o.M1 = mv.M1;
                o.H2 = mv.H2;
                o.M2 = mv.M2;
                o.personal = ctx.Personal.Find(mv.SotrCurrent.Id);
                ctx.SaveChanges();
            }
        }
        public RelayCommand DelSotrCommand
        {
            get { return new RelayCommand(DelSotrProc); }
        }

        private void DelSotrProc(object obj)
        {
            VmGraphic o = Current;
            ctx.Graphic.Remove(o.Obj);
            ctx.SaveChanges();
            this.Remove(o);
        }

        public RelayCommand CopyWeekCommand
        {
            get { return new RelayCommand(s => parent.CopyWeek(Dt)); }
        }

        public void AddGraphic(VmGraphic graphic)
        {
            var o = new Graphic();
            o.Dt = Dt;
            o.H1 = graphic.H1;
            o.M1 = graphic.M1;
            o.H2 = graphic.H2;
            o.M2 = graphic.M2;
            o.PersonalId = graphic.PersonalId;
            o.Personal = ctx.Personal.Find(o.PersonalId);
            ctx.Graphic.Add(o);
            ctx.SaveChanges();
            this.Add(new VmGraphic(o));
        }
    }
}

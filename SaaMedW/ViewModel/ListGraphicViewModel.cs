using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW.ViewModel
{
    public class ListGraphicViewModel : ObservableCollection<VmGraphic>
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private ICollectionView view;
        public ListGraphicViewModel()
        {
            view = CollectionViewSource.GetDefaultView(this);
        }
        public VmGraphic Current
        {
            get => view.CurrentItem as VmGraphic;
            set => view.MoveCurrentTo(value);
        }
        public DateTime Dt { get; set; }
        public int Ind { get; set; }
        public RelayCommand AddSotrCommand
        {
            get { return new RelayCommand(AddSotrProc); }
        }
        private void AddSotrProc(object obj)
        {
            int ind = (int)obj;
            var mv = new EditGraphicViewModel();
            mv.SotrCurrent = null;
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
                o.PersonalId = mv.SotrCurrent.Id;
                ctx.SaveChanges();
            }
        }
    }
}

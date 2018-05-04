using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;

namespace SaaMedW.ViewModel
{
    public class PersonalViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        public ObservableCollection<VmPersonal> PersonalList { get; private set; } 
            = new ObservableCollection<VmPersonal>();

        public PersonalViewModel()
        {
            foreach (var o in ctx.Personal.Include(s => s.PersonalSpecialty.Select(x => x.Specialty)))
            {
                PersonalList.Add(new VmPersonal(o));
            }
        }
        public object PersonalSel
        {
            get { return view.CurrentItem; }
            set { view.MoveCurrentTo(value); }
        }
        private ICollectionView view
        {
            get
            {
                return CollectionViewSource.GetDefaultView(PersonalList);
            }
        }
        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddPersonal);
            }
        }

        private void AddPersonal(object obj)
        {
            var modelView = new EditPersonalViewModel();
            var f = new EditPersonal() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                ctx.Personal.Add(modelView.Obj);
                ctx.SaveChanges();
                var pm = new VmPersonal(modelView.Obj);
                PersonalList.Add(pm);
                view.MoveCurrentTo(pm);
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditPersonal);
            }
        }

        private void EditPersonal(object obj)
        {
            if (PersonalSel == null) return;
            var personal = PersonalSel as VmPersonal;
            var modelView = new EditPersonalViewModel(personal.Obj); ;
            var f = new EditPersonal() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                //personal.Fio = modelView.Fio;
                //personal.
                //personal.SetSpecialty1(ctx);
                //personal.Active = modelView.Active;
                //ctx.SaveChanges();
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelPersonal);
            }
        }

        private void DelPersonal(object obj)
        {
            if (PersonalSel == null) return;
            var personal = PersonalSel as VmPersonal;
            ctx.Personal.Remove(personal.Obj);
            ctx.SaveChanges();
            PersonalList.Remove(personal);
        }

    }
}

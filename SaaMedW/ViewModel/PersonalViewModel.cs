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
                var p = new Personal();
                p.Active = modelView.Active;
                p.Fio = modelView.Fio;
                foreach (var o in modelView.SpecialtyListBox)
                {
                    var o1 = ctx.Specialty.Find(o.Id);
                    p.PersonalSpecialty.Add(new PersonalSpecialty { Specialty = o1 });
                }
                ctx.Personal.Add(p);
                ctx.SaveChanges();
                var vp = new VmPersonal(p);
                PersonalList.Add(vp);
                view.MoveCurrentTo(vp);
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
                personal.Fio = modelView.Fio;
                personal.Active = modelView.Active;

                ctx.PersonalSpecialty.RemoveRange(personal.Obj.PersonalSpecialty);
                personal.PersonalSpecialty.Clear();

                foreach (var o in modelView.SpecialtyListBox)
                {
                    var o1 = ctx.Specialty.Find(o.Id);
                    personal.PersonalSpecialty.Add(new PersonalSpecialty { Specialty = o1 });
                }
                ctx.SaveChanges();
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

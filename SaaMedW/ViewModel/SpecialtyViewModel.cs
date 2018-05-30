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
    public class SpecialtyViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<VmSpecialty> m_specialty 
            = new ObservableCollection<VmSpecialty>();
        public SpecialtyViewModel()
        {
            //foreach (var o in ctx.Specialty
            //    .Include(s => s.ChildSpecialties).Include(s => s.ParentSpecialty)
            //    .Where(s => s.ParentId == null)
            //    .OrderBy(s => s.Id))
            //{
            //    m_specialty.Add(new VmSpecialty(o) { Cargo = SelectedItemMethod});
            //}
            //foreach (var o in ctx.Specialty)
            //{
            //    m_specialty.Add(new VmSpecialty(o));
            //}
        }

        private void SelectedItemMethod(VmSpecialty o)
        {
            SpecialtySel = o;
        }

        private VmSpecialty _selectedItem = null;
        // This is public get-only here but you could implement a public setter which
        // also selects the item.
        // Also this should be moved to an instance property on a VM for the whole tree, 
        // otherwise there will be conflicts for more than one tree.
        public VmSpecialty SpecialtySel
        {
            get { return _selectedItem; }
            private set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                }
            }
        }

        public ObservableCollection<VmSpecialty> SpecialtyList
        {
            get => m_specialty;
        }

        private ICollectionView view
        {
            get
            {
                return CollectionViewSource.GetDefaultView(m_specialty);
            }
        }

        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddObject);
            }
        }

        private void AddObject(object obj)
        {
            if (SpecialtySel == null) return;
            var modelView = new VmSpecialty();
            var f = new EditSpecialty() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                modelView.ParentSpecialty = SpecialtySel.Obj;
                modelView.ParentId = SpecialtySel.Id;
                ctx.Specialty.Add(modelView.Obj);
                ctx.SaveChanges();
                SpecialtyList.Add(modelView);
                SpecialtySel = modelView;
                //view.MoveCurrentTo(modelView);
                OnPropertyChanged("SpecialtyList");
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditObject);
            }
        }

        private void EditObject(object obj)
        {
            if (SpecialtySel == null) return;
            var specialty = SpecialtySel as VmSpecialty;
            var specialtyCpy = new VmSpecialty(specialty);
            var f = new EditSpecialty() { DataContext = specialtyCpy };
            if (f.ShowDialog() ?? false)
            {
                specialty.Copy(specialtyCpy);
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelObject);
            }
        }

        private void DelObject(object obj)
        {
            if (SpecialtySel == null) return;
            if (SpecialtySel.ChildSpecialties.Count > 0) return;
            ctx.Specialty.Remove(SpecialtySel.Obj);
            ctx.SaveChanges();
            SpecialtyList.Remove(SpecialtySel);
        }
    }
}

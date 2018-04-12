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
    public class SpecialtyViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<VmSpecialty> m_specialty = new ObservableCollection<VmSpecialty>();
        public SpecialtyViewModel()
        {
            foreach (var o in ctx.Specialty)
            {
                m_specialty.Add(new VmSpecialty(o));
            }
        }
        public object SpecialtySel
        {
            get { return view.CurrentItem; }
            set { view.MoveCurrentTo(value); }
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
            var modelView = new VmSpecialty();
            var f = new EditSpecialty() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                ctx.Specialty.Add(modelView.Obj);
                ctx.SaveChanges();
                SpecialtyList.Add(modelView);
                view.MoveCurrentTo(modelView);
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
            var specialty = SpecialtySel as VmSpecialty;
            ctx.Specialty.Remove(specialty.Obj);
            ctx.SaveChanges();
            SpecialtyList.Remove(specialty);
        }
    }
}

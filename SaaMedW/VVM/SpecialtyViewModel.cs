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

namespace SaaMedW
{
    public class SpecialtyViewModel
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<VmSpecialty> m_specialty 
            = new ObservableCollection<VmSpecialty>();
        private List<Specialty> lst;
        public SpecialtyViewModel()
        {
            lst = ctx.Specialty.ToList();
            foreach(var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                m_specialty.Add(sp);
            }
        }

        private void BuildTree(VmSpecialty sp)
        {
            foreach(var sp0 in lst.Where(s => s.ParentId == sp.Id)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                sp0.ParentSpecialty = sp;
                sp.ChildSpecialties.Add(sp0);
                BuildTree(sp0);
            }
        }

        public void SelectedItemMethod(VmSpecialty o)
        {
            SpecialtySel = o;
        }

        private VmSpecialty _selectedItem = null;
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

        private void AddObject(object obj) => AddSpecialty(false);

        public RelayCommand Add0
        {
            get
            {
                return new RelayCommand(Add0Object);
            }
        }

        private void Add0Object(object obj) => AddSpecialty(true);

        private void AddSpecialty(bool isRoot)
        {
            if (!isRoot && SpecialtySel == null) return;
            var modelView = new EditStringViewModel();
            var f = new EditStringView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                var specialtyNew = new VmSpecialty();
                specialtyNew.Name = modelView.Name;
                specialtyNew.Cargo = SelectedItemMethod;
                if (!isRoot)
                {
                    specialtyNew.ParentSpecialty = SpecialtySel;
                    specialtyNew.ParentId = SpecialtySel.Id;
                    SpecialtySel.ChildSpecialties.Add(specialtyNew);
                }
                else
                {
                    SpecialtyList.Add(specialtyNew);
                }
                var sp = new Specialty() { Name = specialtyNew.Name, ParentId = specialtyNew.ParentId };
                ctx.Specialty.Add(sp);
                ctx.SaveChanges();
                specialtyNew.Id = sp.Id;
                //modelView.IsSelected = true;
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditObject, s => SpecialtySel != null);
            }
        }

        private void EditObject(object obj)
        {
            if (SpecialtySel == null) return;
            var modelView = new EditStringViewModel(SpecialtySel);
            var f = new EditStringView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                SpecialtySel.Name = modelView.Name;
                var sp = ctx.Specialty.Find(SpecialtySel.Id);
                sp.Name = SpecialtySel.Name;
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelObject, 
                    s => SpecialtySel != null && SpecialtySel.ChildSpecialties.Count == 0);
            }
        }

        private void DelObject(object obj)
        {
            if (SpecialtySel == null) return;
            if (SpecialtySel.ChildSpecialties.Count > 0) return;
            var sp = ctx.Specialty.Find(SpecialtySel.Id);
            ctx.Entry(sp).Collection(s => s.PersonalSpecialty).Load();
            ctx.Entry(sp).Collection(s => s.Benefit).Load();
            if (sp.Benefit.Count > 0)
            {
                System.Windows.MessageBox.Show("По данному направлению есть услуги. Удаление невозможно.");
                return;
            }
            if (sp.PersonalSpecialty.Count > 0)
            {
                System.Windows.MessageBox.Show("По данному направлению есть врачи. Удаление невозможно.");
                return;
            }
            ctx.Specialty.Remove(sp);
            ctx.SaveChanges();
            if (SpecialtySel.ParentSpecialty != null)
            {
                SpecialtySel.ParentSpecialty.ChildSpecialties.Remove(SpecialtySel);
            }
            else
            {
                SpecialtyList.Remove(SpecialtySel);
            }
        }

        public void MoveNode(VmSpecialty source, VmSpecialty target)
        {
            if (source.Id == target?.Id) return;
            if (source.ParentSpecialty != null)
            {
                source.ParentSpecialty.ChildSpecialties.Remove(source);
            }
            else
            {
                SpecialtyList.Remove(source);
            }
            if (target != null)
            {
                target.ChildSpecialties.Add(source);
                source.ParentId = target.Id;
                source.ParentSpecialty = target;
            }
            else
            {
                SpecialtyList.Add(source);
                source.ParentId = null;
                source.ParentSpecialty = null;
            }
            var source0 = ctx.Specialty.Find(source.Id);
            source0.ParentId = source.ParentId;
            if (target != null)
            {
                var target0 = ctx.Specialty.Find(target.Id);
                target0.ParentId = target.ParentId;
            }
            ctx.SaveChanges();

        }
    }
}

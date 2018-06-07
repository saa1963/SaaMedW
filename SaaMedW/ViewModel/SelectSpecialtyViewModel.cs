using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class SelectSpecialtyViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<VmSpecialty> m_specialty
            = new ObservableCollection<VmSpecialty>();
        private List<Specialty> lst;
        public SelectSpecialtyViewModel()
        {
            lst = ctx.Specialty.ToList();
            foreach (var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                m_specialty.Add(sp);
            }
        }

        private void BuildTree(VmSpecialty sp)
        {
            foreach (var sp0 in lst.Where(s => s.ParentId == sp.Id)
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

    }
}

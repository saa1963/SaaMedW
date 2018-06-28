using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class SelectBenefitViewModel: ViewModelBase
    {
        private ObservableCollection<VmSpecialty> m_specialty
            = new ObservableCollection<VmSpecialty>();
        private ObservableCollection<VmBenefit> m_lst = new ObservableCollection<VmBenefit>();
        private SaaMedEntities ctx = new SaaMedEntities();
        private List<Specialty> lst;
        public SelectBenefitViewModel()
        {
            lst = ctx.Specialty.ToList();
            foreach (var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                m_specialty.Add(sp);
            }
            RefreshData();
        }
        private void BuildTree(VmSpecialty sp)
        {
            sp.ChildSpecialties.Clear();
            foreach (var sp0 in lst.Where(s => s.ParentId == sp.Id)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                sp0.ParentSpecialty = sp;
                sp.ChildSpecialties.Add(sp0);
                BuildTree(sp0);
            }
        }
        private void RefreshData()
        {
            m_lst.Clear();
            if (SpecialtySel != null)
            {
                foreach (var o in ctx.Benefit.Include("Specialty")
                    .Where(s => s.SpecialtyId == SpecialtySel.Id))
                {
                    var benefit = new VmBenefit(o);
                    m_lst.Add(benefit);
                }
            }
        }

        private void SelectedItemMethod(VmSpecialty o)
        {
            SpecialtySel = o;
            RefreshData();
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
        public ObservableCollection<VmBenefit> BenefitsList
        {
            get { return m_lst; }
        }
        public ObservableCollection<VmSpecialty> SpecialtyList { get => m_specialty; }
        public VmBenefit BenefitSel { get; set; }
    }
}

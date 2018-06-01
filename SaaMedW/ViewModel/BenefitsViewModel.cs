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
    public class BenefitsViewModel : ViewModelBase
    {
        private ObservableCollection<VmSpecialty> m_specialty 
            = new ObservableCollection<VmSpecialty>();
        private ObservableCollection<VmBenefit> m_lst = new ObservableCollection<VmBenefit>();
        private SaaMedEntities ctx = new SaaMedEntities();
        private List<Specialty> lst;
        public BenefitsViewModel()
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
                    benefit.PropertyChanged += Benefit_PropertyChanged;
                    m_lst.Add(benefit);
                }
            }
        }

        private void Benefit_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (new string[] {"Name", "Duration", "Price"}.Contains(e.PropertyName))
            {
                ctx.SaveChanges();
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
        public object BenefitSel { get; set; }
        private ICollectionView view
        {
            get
            {
                return CollectionViewSource.GetDefaultView(BenefitsList);
            }
        }
        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddBenefit);
            }
        }

        private void AddBenefit(object obj)
        {
            var modelView = new EditBenefitViewModel();
            modelView.SpecialtyId = -1;
            var f = new EditBenefit() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                ctx.Benefit.Add(modelView.Obj);
                ctx.SaveChanges();
                ctx.Entry(modelView.Obj).Reference(s => s.Specialty).Load();
                var pm = new VmBenefit(modelView.Obj);
                BenefitsList.Add(pm);
                view.MoveCurrentTo(pm);
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditBenefit);
            }
        }

        private void EditBenefit(object obj)
        {
            if (BenefitSel == null) return;
            var benefit = BenefitSel as VmBenefit;
            var modelView = new EditBenefitViewModel()
            {
                Name = benefit.Name,
                SpecialtyId = benefit.SpecialtyId,
                Duration = benefit.Duration,
                Price = benefit.Price
            };
            var f = new EditBenefit() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                benefit.Name = modelView.Name;
                benefit.SpecialtyId = modelView.SpecialtyId;
                benefit.Duration = modelView.Duration;
                benefit.Price = modelView.Price;
                ctx.SaveChanges();
                //benefit.Specialty = benefit.Specialty;
                benefit.OnPropertyChanged("SpecialtyId");
                benefit.OnPropertyChanged("Specialty");
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelBenefit);
            }
        }

        private void DelBenefit(object obj)
        {
            if (BenefitSel == null) return;
            var benefit = BenefitSel as VmBenefit;
            ctx.Benefit.Remove(benefit.Obj);
            ctx.SaveChanges();
            BenefitsList.Remove(benefit);
        }
    }
}

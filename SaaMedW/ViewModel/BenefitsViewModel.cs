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
    public class BenefitsViewModel : ViewModelBase
    {
        private ObservableCollection<VmBenefit> m_lst = new ObservableCollection<VmBenefit>();
        private SaaMedEntities ctx = new SaaMedEntities();

        public BenefitsViewModel()
        {
            foreach (var o in ctx.Benefit.Include("Specialty"))
            {
                m_lst.Add(new VmBenefit(o));
            }
        }
        public ObservableCollection<VmBenefit> BenefitsList
        {
            get { return m_lst; }
        }
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

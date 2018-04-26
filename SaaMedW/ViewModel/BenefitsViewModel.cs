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
        public object BenefitSel
        {
            get { return view.CurrentItem; }
            set { view.MoveCurrentTo(value); }
        }
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
            //var modelView = new EditPersonalViewModel();
            //modelView.Specialty = null;
            //var f = new EditPersonal() { DataContext = modelView };
            //if (f.ShowDialog() ?? false)
            //{
            //    ctx.Personal.Add(modelView.Obj);
            //    ctx.SaveChanges();
            //    var pm = new VmPersonal(modelView.Obj);
            //    PersonalList.Add(pm);
            //    view.MoveCurrentTo(pm);
            //}
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
            //if (PersonalSel == null) return;
            //var personal = PersonalSel as VmPersonal;
            //var modelView = new EditPersonalViewModel() { Fio = personal.Fio, Specialty = personal.Specialty, Active = personal.Active };
            //var f = new EditPersonal() { DataContext = modelView };
            //if (f.ShowDialog() ?? false)
            //{
            //    personal.Fio = modelView.Fio;
            //    personal.Specialty = modelView.Specialty;
            //    personal.SetSpecialty1(ctx);
            //    personal.Active = modelView.Active;
            //    ctx.SaveChanges();
            //}
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
            //if (PersonalSel == null) return;
            //var personal = PersonalSel as VmPersonal;
            //ctx.Personal.Remove(personal.Obj);
            //ctx.SaveChanges();
            //PersonalList.Remove(personal);
        }
    }
}

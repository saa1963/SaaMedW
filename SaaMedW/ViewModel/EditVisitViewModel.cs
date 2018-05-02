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
    public class EditVisitViewModel : ViewModelBase
    {
        SaaMedEntities ctx = new SaaMedEntities();
        ObservableCollection<Benefit> benefits = new ObservableCollection<Benefit>();

        public EditVisitViewModel()
        {
            var q = ctx.Benefit.OrderBy(s => s.Name);
            foreach(var o in q)
            {
                benefits.Add(o);
            }
        }
        public ObservableCollection<Benefit> BenefitsList
        {
            get => benefits;
        }
        public Benefit BenefitCurrent
        {
            get => benefits_view.CurrentItem as Benefit;
            set
            {
                if (value == null)
                    benefits_view.MoveCurrentToPosition(-1);
                else
                    benefits_view.MoveCurrentTo(value);
            }
        }
        ICollectionView benefits_view
        {
            get => CollectionViewSource.GetDefaultView(benefits);
        }
        public RelayCommand RefreshGrid
        {
            get { return new RelayCommand(RefreshGridProc); }
        }

        private void RefreshGridProc(object obj)
        {
            System.Windows.MessageBox.Show(BenefitCurrent.Name);
        }
    }
}

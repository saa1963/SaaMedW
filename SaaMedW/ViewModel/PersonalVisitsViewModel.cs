using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class PersonalVisitsViewModel : ViewModelBase
    {
        public int PersonalId { get; set; }
        public ObservableCollection<DateIntervalsViewModel> DateIntervals { get; set; }
         = new ObservableCollection<DateIntervalsViewModel>();

        public void Fill()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class DateIntervalsViewModel : ViewModelBase
    {
        public DateTime Dt { get; set; }
        public ObservableCollection<TimeInterval> Intevals { get; set; }
            = new ObservableCollection<TimeInterval>();
    }
}

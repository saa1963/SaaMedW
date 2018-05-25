using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class EditInvoiceViewModel : ViewModelBase
    {
        public DateTime Dt { get; set; }
        public int Num { get; set; }
        public enStatusInvoice Status { get; set; }
        public int PersonId { get; set; }
        public decimal Sm { get; set; }
        public Person Person { get; set; }
        public ObservableCollection<VmInvoiceDetail> ListInvoiceDetail { get; set; }
            = new ObservableCollection<VmInvoiceDetail>();
    }
}

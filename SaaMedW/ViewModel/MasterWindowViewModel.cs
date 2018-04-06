using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class MasterWindowViewModel : ViewModelBase
    {
        public RelayCommand Cmd
        {
            get { return new RelayCommand(App.ActivateView); }
        }

        //public object CmdSend
        //{
        //    get { return new ExecTypes { View = typeof(SendView), ViewModel = typeof(SendViewModel) }; }
        //}

        //public object CmdReceive
        //{
        //    get { return new ExecTypes { View = typeof(ReceiveView), ViewModel = typeof(ReceiveViewModel) }; }
        //}
    }
}

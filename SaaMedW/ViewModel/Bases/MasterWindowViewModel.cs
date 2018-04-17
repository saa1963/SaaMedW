using SaaMedW.View;
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

        //public RelayCommand Users
        //{
        //    get { return new RelayCommand(new Action<object>(ShowEditUserForm)); }
        //}

        //private void ShowEditUserForm(object obj)
        //{
        //    var modelView = new EditUserViewModel();
        //    var f = new frmEditUser() { DataContext = modelView };
        //    var result = f.ShowDialog();
        //    if (result.HasValue && result.Value)
        //    {
        //        var i = 1;
        //    }
        //}

        //public object CmdReceive
        //{
        //    get { return new ExecTypes { View = typeof(ReceiveView), ViewModel = typeof(ReceiveViewModel) }; }
        //}

        public object CmdUsers
        {
            get { return new ExecTypes { View = typeof(UsersView), ViewModel = typeof(UsersViewModel) }; }
        }
        public object CmdCards
        {
            get { return new ExecTypes { View = typeof(CardsView), ViewModel = typeof(CardsViewModel) }; }
        }
        public object CmdSpec
        {
            get { return new ExecTypes { View = typeof(SpecialtyView), ViewModel = typeof(SpecialtyViewModel) }; }
        }
        public object CmdSotrud
        {
            get { return new ExecTypes { View = typeof(PersonalView), ViewModel = typeof(PersonalViewModel) }; }
        }
        public object CmdGraphic
        {
            get { return new ExecTypes { View = typeof(GraphicView), ViewModel = typeof(GraphicViewModel) }; }
        }
    }
}

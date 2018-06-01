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
        public object CmdBenefits
        {
            get { return new ExecTypes { View = typeof(BenefitsView), ViewModel = typeof(BenefitsViewModel) }; }
        }
        public object CmdNewVisit
        {
            get { return new ExecTypes { View = typeof(EditVisitView), ViewModel = typeof(EditVisitViewModel) }; }
        }
        public object CmdVisit
        {
            get { return new ExecTypes { View = typeof(VisitView), ViewModel = typeof(VisitViewModel) }; }
        }
    }
}

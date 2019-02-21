using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class MasterWindowViewModel
    {
        public RelayCommand Cmd
        {
            get { return new RelayCommand(App.ActivateView); }
        }
        public RelayCommand FrOptionsCommand
        {
            get { return new RelayCommand(FrOptions, s => Global.Source.Fptr != null); }
        }

        private void FrOptions(object obj)
        {
            var atol = new Atol(Global.Source.Fptr);
            string options = atol.ShowProperties();
            if (options != null)
            {
                var compId = Global.Source.GetMotherboardId();
                using (var ctx = new SaaMedEntities())
                {
                    var opt = ctx.Options.Find(
                        new object[] 
                        {
                            enumParameterType.Настройки_ФР,
                            0,
                            compId
                        });
                    if (opt != null)
                    {
                        opt.SetObject(options);
                    }
                    else
                    {
                        var o = new Options()
                        {
                            CompId = compId,
                            ParameterType = enumParameterType.Настройки_ФР,
                            Profile = enumProfile.ЛокальныйВсеПользователи,
                            UserId = 0,
                            ParameterValue = options
                        };
                        ctx.Options.Add(o);
                    }
                    ctx.SaveChanges();
                }
            }
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
        public object CmdVisit
        {
            get { return new ExecTypes { View = typeof(VisitView), ViewModel = typeof(VisitViewModel) }; }
        }
        public object CmdInvoice
        {
            get { return new ExecTypes { View = typeof(InvoiceView), ViewModel = typeof(InvoiceViewModel) }; }
        }
        public object OptionsCommand
        {
            get { return new ExecTypes { View = typeof(OptionsView), ViewModel = typeof(OptionsViewModel) }; }
        }
    }
}

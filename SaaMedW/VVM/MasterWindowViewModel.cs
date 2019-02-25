using SaaMedW.Service;
using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SaaMedW
{
    public class MasterWindowViewModel
    {
        public RelayCommand Cmd
        {
            get { return new RelayCommand(App.ActivateView); }
        }

        public RelayCommand ZReportCommand
        {
            get
            {
                return new RelayCommand(ZReport, 
                    s => ServiceLocator.Instance.GetService<IKkm>().IsInitialized);
            }
        }

        private void ZReport(object obj)
        {
            var kkt = ServiceLocator.Instance.GetService<IKkm>();
            if (kkt.ZReport())
            {
                MessageBox.Show("Смена закрыта");
            }
            else
            {
                MessageBox.Show("Ошибка закрытия смены");
            }
        }

        public RelayCommand FrOptionsCommand
        {
            get { return new RelayCommand(FrOptions, 
                s => ServiceLocator.Instance.GetService<IKkm>() is AtolService); }
        }

        private void FrOptions(object obj)
        {
            var atol = (AtolService)ServiceLocator.Instance.GetService<IKkm>();
            string options = AtolService.ShowProperties();
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
                    if (!atol.Init())
                    {
                        MessageBox.Show("Ошибка инициализации драйвера ККТ");
                    }
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

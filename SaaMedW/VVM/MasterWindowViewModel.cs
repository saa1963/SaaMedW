using log4net;
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
    public class MasterWindowViewModel: NotifyPropertyChanged
    {
        ILog log;
        public MasterWindowViewModel()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
        }
        public RelayCommand Cmd
        {
            get { return new RelayCommand(App.ActivateView); }
        }

        public RelayCommand OpenShiftCommand
        {
            get
            {
                return new RelayCommand(OpenShift,
                    s => ServiceLocator.Instance.GetService<IKkm>().IsInitialized);
            }
        }

        private void OpenShift(object obj)
        {
            var kkt = ServiceLocator.Instance.GetService<IKkm>();
            if (kkt.OpenShift())
            {
                MessageBox.Show("Смена открыта");
            }
            else
            {
                MessageBox.Show("Ошибка открытия смены");
            }
            SetTitle();
        }

        public RelayCommand DataCheckCommand
        {
            get
            {
                return new RelayCommand(DataCheck,
                    s => ServiceLocator.Instance.GetService<IKkm>().IsInitialized);
            }
        }

        private void DataCheck(object obj)
        {
            var modelView = new EditGetCheckdataViewModel();
            var f = new EditGetCheckdataView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                var kkt = ServiceLocator.Instance.GetService<IKkm>();
                if (((AtolService)kkt).ReadCheck(num: modelView.Nfd))
                    MessageBox.Show("Расчет закончен");
                else
                    MessageBox.Show("Ошибка чтения документа");
            }
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
            SetTitle();
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
        public object CmdMkb
        {
            get { return new ExecTypes { View = typeof(MkbView), ViewModel = typeof(MkbViewModel) }; }
        }
        public object CmdZakaz
        {
            get { return new ExecTypes { View = typeof(ZakazView), ViewModel = typeof(ZakazViewModel) }; }
        }
        public object OptionsCommand
        {
            get { return new ExecTypes { View = typeof(OptionsView), ViewModel = typeof(OptionsViewModel) }; }
        }
        private string m_Title;
        public string Title
        {
            get => m_Title;
            set
            {
                m_Title = value;
                OnPropertyChanged("Title");
            }
        }
        public bool IsAdmin => Global.Source.RUser.Role == 0;

        public void SetTitle()
        {
            var fptr = ServiceLocator.Instance.GetService<IKkm>();
            if (fptr != null)
            {
                int numShift = fptr.GetNumShift();
                if (numShift >= 0)
                {
                    Title = $"Регистратура. (Кассир - {Global.Source.RUser.Fio})(Номер открытой смены {numShift})";
                }
                else if (numShift == -1)
                {
                    Title = $"Регистратура. (Кассир - {Global.Source.RUser.Fio})(Смена закрыта)";
                }
                else if (numShift == -2)
                {
                    Title = $"Регистратура. (Кассир - {Global.Source.RUser.Fio})(Смена истекла > 24 часов)";
                }
                else if (numShift == -3)
                {
                    Title = $"Регистратура. (Кассир - {Global.Source.RUser.Fio})(Состояние смены неизвестно)";
                }
            }
            else
            {
                Title = $"Регистратура. (Кассир - {Global.Source.RUser.Fio})(Состояние смены неизвестно)";
            }
        }

        public RelayCommand DmsCompanyCommand
            => new RelayCommand(DmsCompanyClassificator);

        private void DmsCompanyClassificator(object obj)
        {
            var modelView = new DmsCompanyViewModel(new SaaMedEntities());
            var f = new DmsCompanyView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {

            }
        }
    }
}

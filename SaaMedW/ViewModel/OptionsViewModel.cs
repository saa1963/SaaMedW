using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class OptionsViewModel: ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private ObservableCollection<Options> m_CommonParameterList
            = new ObservableCollection<Options>();
        private ObservableCollection<Options> m_UserParameterList
            = new ObservableCollection<Options>();
        private ObservableCollection<Options> m_ComputerParameterList
            = new ObservableCollection<Options>();
        private ObservableCollection<Options> m_UserComputerParameterList
            = new ObservableCollection<Options>();
        public ObservableCollection<Options> CommonParameterList
        {
            get => m_CommonParameterList;
            set
            {
                m_CommonParameterList = value;
                OnPropertyChanged("CommonParameterList");
            }
        }
        public ObservableCollection<Options> UserParameterList
        {
            get => m_UserParameterList;
            set
            {
                m_UserParameterList = value;
                OnPropertyChanged("UserParameterList");
            }
        }
        public ObservableCollection<Options> ComputerParameterList
        {
            get => m_ComputerParameterList;
            set
            {
                m_ComputerParameterList = value;
                OnPropertyChanged("ComputerParameterList");
            }
        }
        public ObservableCollection<Options> UserComputerParameterList
        {
            get => m_UserComputerParameterList;
            set
            {
                m_UserComputerParameterList = value;
                OnPropertyChanged("UserComputerParameterList");
            }
        }
        public OptionsViewModel()
        {
            var compId = Global.Source.GetMotherboardId();
            // Общие настройки
            if (Global.Source.rUser.Role == 0)
            {
                foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.Общий))
                {
                    var nv = new Options()
                    {
                        ParameterType = o.Key,
                        Profile = enumProfile.Общий,
                        CompId = "0",
                        UserId = 0
                    };
                    nv.SetObject(Options.GetParameter<object>(o.Key));
                    m_CommonParameterList.Add(nv);
                }
            }
            // Перемещаемые настройки пользователя
            foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.ПеремещаемыйПользователя))
            {
                var nv = new Options()
                {
                    ParameterType = o.Key,
                    Profile = enumProfile.ПеремещаемыйПользователя,
                    CompId = "0",
                    UserId = Global.Source.rUser.Id
                };
                nv.SetObject(Options.GetParameter<object>(o.Key));
                m_UserParameterList.Add(nv);
            }
            // Локальные настройки для всех пользователей
            foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.ЛокальныйВсеПользователи))
            {
                var nv = new Options()
                {
                    ParameterType = o.Key,
                    Profile = enumProfile.ЛокальныйВсеПользователи,
                    CompId = compId,
                    UserId = 0
                };
                nv.SetObject(Options.GetParameter<object>(o.Key));
                m_ComputerParameterList.Add(nv);
            }
            // Локальные настройки пользователя
            foreach (var o in Options.ВсеВидыПараметров.Where(s => s.Value.profile == enumProfile.ЛокальныйПользователя))
            {
                var nv = new Options()
                {
                    ParameterType = o.Key,
                    Profile = enumProfile.ЛокальныйПользователя,
                    CompId = compId,
                    UserId = Global.Source.rUser.Id
                };
                nv.SetObject(Options.GetParameter<object>(o.Key));
                m_UserComputerParameterList.Add(nv);
            }
        }
    }
}

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
        private ObservableCollection<NameValue> m_CommonParameterList
            = new ObservableCollection<NameValue>();
        private ObservableCollection<NameValue> m_UserParameterList
            = new ObservableCollection<NameValue>();
        private ObservableCollection<NameValue> m_ComputerParameterList
            = new ObservableCollection<NameValue>();
        private ObservableCollection<NameValue> m_UserComputerParameterList
            = new ObservableCollection<NameValue>();
        public ObservableCollection<NameValue> CommonParameterList
        {
            get => m_CommonParameterList;
            set
            {
                m_CommonParameterList = value;
                OnPropertyChanged("CommonParameterList");
            }
        }
        public ObservableCollection<NameValue> UserParameterList
        {
            get => m_UserParameterList;
            set
            {
                m_UserParameterList = value;
                OnPropertyChanged("UserParameterList");
            }
        }
        public ObservableCollection<NameValue> ComputerParameterList
        {
            get => m_ComputerParameterList;
            set
            {
                m_ComputerParameterList = value;
                OnPropertyChanged("ComputerParameterList");
            }
        }
        public ObservableCollection<NameValue> UserComputerParameterList
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
            // Общие настройки
            if (Global.Source.rUser.Role == 0)
            {
                foreach (var o in ctx.Options.Where(s => s.UserId == 0 && s.CompId == "0"))
                {
                    var nv = new NameValue()
                    {
                        Name = Enum.GetName(typeof(enumParameterType), o.ParameterType),
                        Value = o.GetObject()
                    };
                    m_CommonParameterList.Add(nv);
                }
            }
            // Перемещаемые настройки пользователя
            foreach (var o in ctx.Options.Where(s => s.UserId == Global.Source.rUser.Id && s.CompId == "0"))
            {
                var nv = new NameValue()
                {
                    Name = Enum.GetName(typeof(enumParameterType), o.ParameterType),
                    Value = o.GetObject()
                };
                m_UserParameterList.Add(nv);
            }
            // Локальные настройки для всех пользователей
            foreach (var o in ctx.Options.Where(s => s.UserId == 0 
                && s.CompId == Global.Source.GetMotherboardId()))
            {
                var nv = new NameValue()
                {
                    Name = Enum.GetName(typeof(enumParameterType), o.ParameterType),
                    Value = o.GetObject()
                };
                m_ComputerParameterList.Add(nv);
            }
            // Локальные настройки пользователя
            foreach (var o in ctx.Options.Where(s => s.UserId == Global.Source.rUser.Id 
                && s.CompId == Global.Source.GetMotherboardId()))
            {
                var nv = new NameValue()
                {
                    Name = Enum.GetName(typeof(enumParameterType), o.ParameterType),
                    Value = o.GetObject()
                };
                m_UserComputerParameterList.Add(nv);
            }
        }
    }
}

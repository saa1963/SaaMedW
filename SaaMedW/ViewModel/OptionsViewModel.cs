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
        public OptionsViewModel()
        {
            if (Global.Source.rUser.Role == 0)
            {
                ctx.Options.Where(s => s.UserId == 0)
            }
        }
    }
}

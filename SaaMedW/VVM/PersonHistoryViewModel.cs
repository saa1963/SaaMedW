using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW
{
    public class PersonHistoryViewModel: NotifyPropertyChanged
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private ObservableCollection<VmZakaz> m_ZakazList = new ObservableCollection<VmZakaz>();
        public ObservableCollection<VmZakaz> ZakazList
        {
            get => m_ZakazList;
            set
            {
                m_ZakazList = value;
                OnPropertyChanged("ZakazList");
            }
        }
        private string m_Fio;
        public string Fio
        {
            get => m_Fio;
            set
            {
                m_Fio = value;
                OnPropertyChanged("Fio");
            }
        }
        public PersonHistoryViewModel(Person person)
        {
            Fio = person.Fio;
            foreach(var z in ctx.Zakaz.Where(s => s.PersonId == person.Id && s.Vozvrat == null).OrderByDescending(s => s.Dt))
            {
                m_ZakazList.Add(new VmZakaz(z));
            }
        }
    }
}

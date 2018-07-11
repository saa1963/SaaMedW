using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class PersonInfoViewModel: ViewModelBase
    {
        private Person person;
        private SaaMedEntities ctx = new SaaMedEntities();
        private List<Person> m_PersonList;
        public List<Person> PersonList
        {
            get => m_PersonList;
        }
        public Person PersonSel { get; set; }
        public ObservableCollection<IdName> SelectedList { get; set; }
        public IdName InfoSel { get; set; }
        public PersonInfoViewModel(Person p_person)
        {
            person = p_person;
            m_PersonList = ctx.Person.OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName).ThenBy(s => s.MiddleName).ToList();
            SelectedList = new ObservableCollection<IdName>();
            foreach (var o in person.Person_Person2)
            {
                SelectedList.Add(new IdName { Id = o.Id, Name = o.FioBirthday });
            }
        }
        public RelayCommand AddInfoCommand
        {
            get => new RelayCommand(AddInfo, s => PersonSel != null 
                && SelectedList.SingleOrDefault(s0 => s0.Id == PersonSel.Id) == null
                && PersonSel.Id != person.Id);
        }

        private void AddInfo(object obj)
        {
            SelectedList.Add(new IdName { Id = PersonSel.Id, Name = PersonSel.FioBirthday });
        }

        public RelayCommand DelInfoCommand
        {
            get => new RelayCommand(DelInfo, s => InfoSel != null);
        }

        private void DelInfo(object obj)
        {
            SelectedList.Remove(InfoSel);
        }
    }
}

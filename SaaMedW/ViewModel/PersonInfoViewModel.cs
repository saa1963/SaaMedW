using System;
using System.Collections.Generic;
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
        public List<IdName> SelectedList { get; set; }
        public List<Person> PersonList
        {
            get => m_PersonList;
        }
        public PersonInfoViewModel(Person p_person)
        {
            person = p_person;
            m_PersonList = ctx.Person.OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName).ThenBy(s => s.MiddleName).ToList();
        }
    }
}

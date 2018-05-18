using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class EditOneVisitViewModel: ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private int m_PersonId;
        private int m_PersonalId;
        private DateTime m_Dt;
        private int m_Duration;
        private int m_Status;

        public List<Person> ListPerson { get; private set; }
        public List<Personal> ListPersonal { get; private set; }
        public List<IdName> ListStatus { get; private set; } = new List<IdName>();
        public EditOneVisitViewModel()
        {
            FillPerson();
            FillPersonal();
            ListStatus.Add(new IdName() { Id = 0, Name = "Предварительный" });
            ListStatus.Add(new IdName() { Id = 1, Name = "Завершен" });
        }

        private void FillPersonal()
        {
            ListPersonal = ctx.Personal.Where(s => s.Active).OrderBy(s => s.Fio).ToList();
        }

        private void FillPerson()
        {
            ListPerson = ctx.Person.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                .ThenBy(s => s.MiddleName).ToList();
        }
        public EditOneVisitViewModel(Visit visit):this()
        {
            PersonId = visit.PersonId;
            PersonalId = visit.PersonalId;
            Dt = visit.Dt;
            Duration = visit.Duration;
            Status = visit.Status;
        }
        public int PersonId
        {
            get => m_PersonId;
            set { m_PersonId = value; OnPropertyChanged("PersonId"); }
        }
        public int PersonalId
        {
            get => m_PersonalId;
            set { m_PersonalId = value; OnPropertyChanged("PersonalId"); }
        }
        public DateTime Dt
        {
            get => m_Dt.Date;
            set
            {
                m_Dt = value.AddHours(m_Dt.Hour).AddMinutes(m_Dt.Minute);
                OnPropertyChanged("Dt");
            }
        }
        public int H1
        {
            get => m_Dt.Hour;
            set
            {
                m_Dt = m_Dt.AddHours(value).AddMinutes(m_Dt.Minute);
                OnPropertyChanged("H1");
            }
        }
        public int M1
        {
            get => m_Dt.Minute;
            set
            {
                m_Dt = m_Dt.AddHours(m_Dt.Hour).AddMinutes(value);
                OnPropertyChanged("M1");
            }
        }
        public int Duration
        {
            get => m_Duration;
            set { m_Duration = value; OnPropertyChanged("Duration"); }
        }
        public int Status
        {
            get => m_Status;
            set
            {
                m_Status = value;
                OnPropertyChanged("Status");
            }
        }
    }
}

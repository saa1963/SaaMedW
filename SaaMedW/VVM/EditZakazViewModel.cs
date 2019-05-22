using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SaaMedW
{
    class EditZakazViewModel : NotifyPropertyChanged, IDataErrorInfo
    {
        SaaMedEntities ctx = new SaaMedEntities();
        public EditZakazViewModel(int personId)
        {
            Person = ctx.Person.Find(personId);
            foreach (var p in ctx.Person.OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName).ThenBy(s => s.MiddleName)
                .ThenBy(s => s.BirthDate))
            {
                PersonList.Add(p);
            }
            foreach (var p in ctx.DmsCompany.OrderBy(s => s.Name))
            {
                DmsCompanyList.Add(p);
            }
        }
        private ObservableCollection<Person> m_PersonList = new ObservableCollection<Person>();
        public ObservableCollection<Person> PersonList
        {
            get => m_PersonList;
            set
            {
                m_PersonList = value;
                OnPropertyChanged("PersonList");
            }
        }
        private ObservableCollection<DmsCompany> m_DmsCompanyList = new ObservableCollection<DmsCompany>();
        public ObservableCollection<DmsCompany> DmsCompanyList
        {
            get => m_DmsCompanyList;
            set
            {
                m_DmsCompanyList = value;
                OnPropertyChanged("DmsCompanyList");
            }
        }
        private int m_Num;
        public int Num
        {
            get => m_Num;
            set
            {
                m_Num = value;
                OnPropertyChanged("Num");
            }
        }
        private DateTime m_Dt;
        public DateTime Dt
        {
            get => m_Dt;
            set
            {
                m_Dt = value;
                OnPropertyChanged("Dt");
            }
        }
        private Person m_Person;
        public Person Person
        {
            get => m_Person;
            set
            {
                m_Person = value;
                OnPropertyChanged("Person");
            }
        }
        private bool m_Dms;
        public bool Dms
        {
            get => m_Dms;
            set
            {
                m_Dms = value;
                OnPropertyChanged("Dms");
            }
        }
        private string m_Polis;
        public string Polis
        {
            get => m_Polis;
            set
            {
                m_Polis = value;
                OnPropertyChanged("Polis");
            }
        }
        private DmsCompany m_DmsCompany;
        public DmsCompany DmsCompany
        {
            get => m_DmsCompany;
            set
            {
                m_DmsCompany = value;
                OnPropertyChanged("DmsCompany");
            }
        }
        public RelayCommand DmsCompanyClassificatorCommand
            => new RelayCommand(DmsCompanyClassificatorOpen);

        private void DmsCompanyClassificatorOpen(object obj)
        {
            var modelView = new DmsCompanyViewModel(ctx);
            var f = new DmsCompanyView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {

            }
        }

        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        //if (String.IsNullOrWhiteSpace(Name))
                        //    result = "Не введено наименование.";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }

        public string Error => "";
    }
}

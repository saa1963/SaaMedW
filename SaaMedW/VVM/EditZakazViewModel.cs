﻿using System;
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
        }
        private List<Person> m_PersonList = new List<Person>();
        public List<Person> PersonList
        {
            get => m_PersonList;
            set
            {
                m_PersonList = value;
                OnPropertyChanged("PersonList");
            }
        }
        public Visibility VisibilitySearchPerson
        {
            get
            {
                if (String.IsNullOrWhiteSpace(SearchPerson))
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }
        private string m_SearchPerson;
        public string SearchPerson
        {
            get => m_SearchPerson;
            set
            {
                m_SearchPerson = value;
                PersonList = ctx.Person.Where(s => s.Fio.StartsWith(m_SearchPerson)).ToList();
                OnPropertyChanged("VisibilitySearchPerson");
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

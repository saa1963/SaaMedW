﻿using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using log4net;
using System.Windows;
using System.Data.SqlClient;

namespace SaaMedW
{
    public class CardsViewModel : NotifyPropertyChanged
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private ILog log;

        public CardsViewModel()
        {
            log = LogManager.GetLogger(this.GetType());
        }
        private void RefreshData()
        {
            IQueryable<Person> expr;
            CardsList.Clear();
            if (!String.IsNullOrWhiteSpace(m_SearchText))
            {
                expr = ctx.Person.Where(s => s.LastName.ToUpper()
                    .Contains(m_SearchText.ToUpper())).Include(s => s.DocumentType);
                
                foreach (var o in expr)
                {
                    CardsList.Add(new VmPerson(o));
                }
            }
        }
        public ObservableCollection<VmPerson> CardsList { get; } 
            = new ObservableCollection<VmPerson>();

        public VmPerson CardsSel { get; set; }
        private string m_SearchText = "";
        public string SearchText
        {
            get => m_SearchText;
            set
            {
                if (value != m_SearchText)
                {
                    m_SearchText = value;
                    OnPropertyChanged("SearchText");
                }
            }
        }
        private ICollectionView viewUsers => CollectionViewSource.GetDefaultView(CardsList);

        public RelayCommand Add => new RelayCommand(AddPerson);

        private void AddPerson(object obj)
        {
            var modelView = new EditPersonViewModel();
            var f = new EditPersonView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                var o = new VmPerson()
                {
                    AddressCity = modelView.AddressCity,
                    AddressFlat = modelView.AddressFlat,
                    AddressHouse = modelView.AddressHouse,
                    AddressPunkt = modelView.AddressPunkt,
                    AddressRaion = modelView.AddressRaion,
                    AddressStreet = modelView.AddressStreet,
                    AddressSubject = modelView.AddressSubject,
                    BirthDate = modelView.BirthDate,
                    DocNumber = modelView.DocNumber,
                    DocSeria = modelView.DocSeria,
                    DocumentTypeId = modelView.DocumentTypeId,
                    FirstName = modelView.FirstName,
                    Inn = modelView.Inn,
                    LastName = modelView.LastName,
                    Mestnost = modelView.Mestnost,
                    MiddleName = modelView.MiddleName,
                    Phone = modelView.Phone,
                    Sex = modelView.Sex,
                    Snils = modelView.Snils,
                    Polis = modelView.Polis,
                    DmsCompanyId = modelView.DmsCompanyId
                };
                ctx.Person.Add(o.Obj);
                ctx.SaveChanges();
                CardsList.Add(o);
                viewUsers.MoveCurrentTo(o);
            }
        }

        public RelayCommand Edit => new RelayCommand(EditPerson, s => CardsSel != null);

        private void EditPerson(object obj)
        {
            if (CardsSel == null) return;
            var modelView = new EditPersonViewModel(CardsSel.Obj);
            var f = new EditPersonView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                CardsSel.AddressCity = modelView.AddressCity;
                CardsSel.AddressFlat = modelView.AddressFlat;
                CardsSel.AddressHouse = modelView.AddressHouse;
                CardsSel.AddressPunkt = modelView.AddressPunkt;
                CardsSel.AddressRaion = modelView.AddressRaion;
                CardsSel.AddressStreet = modelView.AddressStreet;
                CardsSel.AddressSubject = modelView.AddressSubject;
                CardsSel.BirthDate = modelView.BirthDate;
                CardsSel.DocNumber = modelView.DocNumber;
                CardsSel.DocSeria = modelView.DocSeria;
                CardsSel.DocumentTypeId = modelView.DocumentTypeId;
                CardsSel.FirstName = modelView.FirstName;
                CardsSel.Inn = modelView.Inn;
                CardsSel.LastName = modelView.LastName;
                CardsSel.Mestnost = modelView.Mestnost;
                CardsSel.MiddleName = modelView.MiddleName;
                CardsSel.Phone = modelView.Phone;
                CardsSel.Sex = modelView.Sex;
                CardsSel.Snils = modelView.Snils;
                CardsSel.Polis = modelView.Polis;
                CardsSel.DmsCompany = ctx.DmsCompany.Find(modelView.DmsCompanyId);
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del => new RelayCommand(DelPerson, s => CardsSel != null);

        private void DelPerson(object obj)
        {
            try
            {
                if (CardsSel == null) return;
                VmPerson person = CardsSel;
                ctx.Person.Remove(person.Obj);
                ctx.SaveChanges();
                CardsList.Remove(person);
            }
            catch (Exception e) when (e.InnerException?.InnerException != null)
            {
                if (e.InnerException.InnerException is SqlException)
                {
                    var ex = e.InnerException.InnerException as SqlException;
                    if (ex.Number == 547)
                    {
                        MessageBox.Show("Есть ссылки на клиента. Удаление невозможно.");
                    }
                    else
                    {
                        throw e;
                    }
                }
            }
        }

        public RelayCommand MedCard => new RelayCommand(PrintMedCard, s => CardsSel != null);

        private void PrintMedCard(object obj)
        {
            if (CardsSel == null) return;
            VmPerson person = CardsSel;
            var fname = new MedCard().DoIt(person.Obj);
            System.Diagnostics.Process.Start(fname);
        }

        public RelayCommand Vmesh => new RelayCommand(PrintVmesh, s => CardsSel != null);

        private void PrintVmesh(object obj)
        {
            if (CardsSel == null) return;
            VmPerson person = CardsSel;
            var fname = new Vmesh().DoIt(DateTime.Now, person.Obj);
            System.Diagnostics.Process.Start(fname);
        }

        public RelayCommand PersonsInfoCommand => new RelayCommand(PersonsInfo, s => CardsSel != null);

        public RelayCommand NewVisitCommand => new RelayCommand(NewVisit, s => CardsSel != null);

        private void NewVisit(object obj)
        {
            try
            {
                if (CardsSel == null) return;
                var viewModel = new EditOneVisitViewModel(ctx, CardsSel.Obj);
                var f = new EditOneVisitView() { DataContext = viewModel };
                if (f.ShowDialog() ?? false)
                {
                    var o = new Visit()
                    {
                        Dt = viewModel.IntervalSel.Begin,
                        Duration = viewModel.IntervalSel.Interval.Minutes,
                        Person = CardsSel.Obj,
                        Personal = viewModel.PersonalSel.Obj,
                        Status = false,
                        VisitBenefit = viewModel.VisitBenefit,
                        NumDog = Options.GetParameter<int>(enumParameterType.Номер_договора)
                    };
                    ctx.Visit.Add(o);
                    ctx.SaveChanges();
                    Options.SetParameter<int>(enumParameterType.Номер_договора, o.NumDog.Value + 1);
                }
            }
            catch (Exception e)
            {
                var msg = "Ошибка создания посещения.";
                log.Error(msg, e);
                MessageBox.Show(msg);
            }
        }

        public RelayCommand ZakazCommand => new RelayCommand(NewZakaz, s => CardsSel != null);

        private void NewZakaz(object obj)
        {
            VmPerson person = CardsSel;
            var viewModel = new EditZakazViewModel(person.Id, person.DmsCompany?.Id)
            {
                Num = Options.GetParameter<int>(enumParameterType.Номер_договора),
                Dt = DateTime.Today,
                Dms = !String.IsNullOrWhiteSpace(person.Polis) || person.DmsCompanyId.HasValue,
                Polis = person.Polis
            };
            var f = new EditZakazView() { DataContext = viewModel };
            f.ShowDialog();
        }

        private void PersonsInfo(object obj)
        {
            if (CardsSel == null) return;
            VmPerson person = CardsSel;
            var viewModel = new PersonInfoViewModel(person.Obj);
            var f = new PersonInfoView() { DataContext = viewModel };
            if (f.ShowDialog() ?? false)
            {
                person.Obj.Person_Person2.Clear();
                foreach (var o in viewModel.SelectedList)
                {
                    person.Obj.Person_Person2.Add(ctx.Person.Find(o.Id));
                }
                ctx.SaveChanges();
            }
        }

        public RelayCommand DelSearchCommand => new RelayCommand(DelSearch, s => (m_SearchText ?? "").Length > 0 );

        private void DelSearch(object obj)
        {
            SearchText = "";
            RefreshData();
        }

        public RelayCommand SearchCommand => new RelayCommand(Search, s => (m_SearchText ?? "").Length > 0);

        private void Search(object obj)
        {
            RefreshData();
        }

        public RelayCommand PersonHistoryCommand => new RelayCommand(PersonHistory, s => CardsSel != null);

        private void PersonHistory(object obj)
        {
            var vm = new PersonHistoryViewModel(CardsSel.Obj);
            var f = new PersonHistoryView() { DataContext = vm };
            f.ShowDialog();
        }
    }
}

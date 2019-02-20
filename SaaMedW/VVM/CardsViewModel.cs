using SaaMedW.View;
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
                    Snils = modelView.Snils
                };
                ctx.Person.Add(o.Obj);
                ctx.SaveChanges();
                CardsList.Add(o);
                viewUsers.MoveCurrentTo(o);
            }
        }

        public RelayCommand Edit => new RelayCommand(EditPerson);

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
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del => new RelayCommand(DelPerson);

        private void DelPerson(object obj)
        {
            if (CardsSel == null) return;
            VmPerson person = CardsSel;
            ctx.Person.Remove(person.Obj);
            ctx.SaveChanges();
            CardsList.Remove(person);
        }

        public RelayCommand MedCard => new RelayCommand(PrintMedCard);

        private void PrintMedCard(object obj)
        {
            if (CardsSel == null) return;
            VmPerson person = CardsSel;
            new MedCard().DoIt(person.Obj);
        }

        public RelayCommand Vmesh => new RelayCommand(PrintVmesh);

        private void PrintVmesh(object obj)
        {
            if (CardsSel == null) return;
            VmPerson person = CardsSel;
            new Vmesh().DoIt(person.Obj);
        }

        public RelayCommand PersonsInfoCommand => new RelayCommand(PersonsInfo);

        public RelayCommand NewVisitCommand => new RelayCommand(NewVisit, s => CardsSel != null);

        private void NewVisit(object obj)
        {
            try
            {
                if (CardsSel == null) return;
                var viewModel = new EditOneVisitViewModel(ctx);
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
                        VisitBenefit = viewModel.VisitBenefit
                    };
                    ctx.Visit.Add(o);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                var msg = "Ошибка создания посещения.";
                log.Error(msg, e);
                MessageBox.Show(msg);
            }
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
    }
}

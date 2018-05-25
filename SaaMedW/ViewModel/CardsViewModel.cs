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

namespace SaaMedW.ViewModel
{
    public class CardsViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<EditPersonViewModel> m_cards = new ObservableCollection<EditPersonViewModel>();

        public CardsViewModel()
        {
            foreach (var o in ctx.Person.Include(s => s.DocumentType))
            {
                m_cards.Add(new EditPersonViewModel(o));
            }
        }
        public ObservableCollection<EditPersonViewModel> CardsList
        {
            get { return m_cards; }
        }
        public object CardsSel
        {
            get { return viewUsers.CurrentItem; }
            set { viewUsers.MoveCurrentTo(value); }
        }
        private ICollectionView viewUsers
        {
            get
            {
                return CollectionViewSource.GetDefaultView(CardsList);
            }
        }
        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddPerson);
            }
        }

        private void AddPerson(object obj)
        {
            var modelView = new EditPersonViewModel();
            var f = new EditCards() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                ctx.Person.Add(modelView.Obj);
                ctx.SaveChanges();
                CardsList.Add(modelView);
                viewUsers.MoveCurrentTo(modelView);
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditPerson);
            }
        }

        private void EditPerson(object obj)
        {
            if (CardsSel == null) return;
            var person = CardsSel as EditPersonViewModel;
            var modelView = new EditPersonViewModel(person.Obj);
            var f = new EditCards() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                person.Copy(modelView);
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelPerson);
            }
        }

        private void DelPerson(object obj)
        {
            if (CardsSel == null) return;
            var person = CardsSel as EditPersonViewModel;
            ctx.Person.Remove(person.Obj);
            ctx.SaveChanges();
            CardsList.Remove(person);
        }

        public RelayCommand MedCard
        {
            get
            {
                return new RelayCommand(PrintMedCard);
            }
        }

        private void PrintMedCard(object obj)
        {
            if (CardsSel == null) return;
            var person = CardsSel as EditPersonViewModel;
            new MedCard().DoIt(person.Obj);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW.ViewModel
{
    public class EditPersonalViewModel : VmPersonal, IDataErrorInfo
    {
        SaaMedEntities ctx;
        public ObservableCollection<VmSpecialty> SpecialtyCombo { get; private set; }
            = new ObservableCollection<VmSpecialty>();
        public ObservableCollection<VmSpecialty> SpecialtyListBox { get; private set; }
            = new ObservableCollection<VmSpecialty>();

        public EditPersonalViewModel(SaaMedEntities _ctx): base()
        {
            ctx = _ctx;
            FillSpecialty();
        }
        public EditPersonalViewModel(SaaMedEntities _ctx, Personal obj)
        {
            ctx = _ctx;
            Obj = new Personal();
            Active = obj.Active;
            Fio = obj.Fio;
            Id = obj.Id;
            foreach (var o in obj.PersonalSpecialty)
            {
                PersonalSpecialty.Add(o);
            }
            FillSpecialty();
            foreach (var o in PersonalSpecialty)
            {
                SpecialtyListBox.Add(new VmSpecialty(o.Specialty));
            }
        }
        private void FillSpecialty()
        {
            foreach (Specialty o in ctx.Specialty)
            {
                SpecialtyCombo.Add(new VmSpecialty(o));
            }
        }
        public int SelectedCombo { get; set; }
        public int SelectedListBox { get; set; }
        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    case "Fio":
                        if (String.IsNullOrWhiteSpace(Fio))
                            result = "Не введена ФИО.";
                        break;
                    case "SpecialtyListBox":
                        if (SpecialtyListBox.Count == 0)
                            result = "Не выбраны специальности";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error
        {
            get => null;
        }
        public RelayCommand AddSpecialtyCommand
        {
            get => new RelayCommand(AddSpecialty, (x) => SelectedCombo > 0);
        }
        private void AddSpecialty(object obj)
        {
            var o = SpecialtyCombo.Single(s => s.Id == SelectedCombo);
            if (!SpecialtyListBox.Contains(o))
            {
                SpecialtyListBox.Add(o);
                PersonalSpecialty.Add(new SaaMedW.PersonalSpecialty() { Specialty = o.Obj, Personal = this.Obj });
                OnPropertyChanged("SpecialtyListBox");
            }
        }
        public RelayCommand DelSpecialtyCommand
        {
            get => new RelayCommand(DelSpecialty, (x) => SelectedListBox > 0);
        }
        private void DelSpecialty(object obj)
        {
            SpecialtyListBox.Remove(SpecialtyListBox.Single(s => s.Id == SelectedListBox));
            PersonalSpecialty.Remove(PersonalSpecialty.Single(s => s.SpecialtyId == SelectedListBox));
            OnPropertyChanged("SpecialtyListBox");
        }
    }
}

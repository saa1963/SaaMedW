using System;
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
        private Personal m_object;
        private SaaMedEntities ctx = new SaaMedEntities();

        public ObservableCollection<VmSpecialty> SpecialtyCombo { get; private set; } 
            = new ObservableCollection<VmSpecialty>();
        public ObservableCollection<VmSpecialty> SpecialtyListBox { get; private set; } 
            = new ObservableCollection<VmSpecialty>();

        public EditPersonalViewModel()
        {
            m_object = new Personal();
            FillSpecialty();
        }
        public EditPersonalViewModel(Personal obj)
        {
            m_object = new Personal();
            m_object.Active = obj.Active;
            m_object.Fio = obj.Fio;
            m_object.Id = obj.Id;
            foreach (var o in obj.PersonalSpecialty)
            {
                m_object.PersonalSpecialty.Add(o);
            }
            FillSpecialty();
            foreach (var o in m_object.PersonalSpecialty)
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
        //private ICollectionView view
        //{
        //    get => CollectionViewSource.GetDefaultView(SpecialtyListBox);
        //}
        //public VmSpecialty CurrentSpecialty
        //{
        //    get
        //    {
        //        return view.CurrentItem as VmSpecialty;
        //    }
        //    set
        //    {
        //        if (value == null)
        //            view.MoveCurrentToPosition(-1);
        //        else
        //            view.MoveCurrentTo(value);
        //    }
        //}
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
            SpecialtyListBox.Add(SpecialtyCombo.Single(s => s.Id == SelectedCombo));
            OnPropertyChanged("SpecialtyListBox");
        }
        public RelayCommand DelSpecialtyCommand
        {
            get => new RelayCommand(DelSpecialty, (x) => SelectedListBox > 0);
        }
        private void DelSpecialty(object obj)
        {
            SpecialtyListBox.Remove(SpecialtyListBox.Single(s => s.Id == SelectedListBox));
            OnPropertyChanged("SpecialtyListBox");
            if (SpecialtyListBox.Count == 0) SelectedListBox = 0;
            
        }
    }
}

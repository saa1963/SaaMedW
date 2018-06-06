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
    public class EditPersonalViewModel : ViewModelBase, IDataErrorInfo
    {
        private List<Specialty> lst;
        private ObservableCollection<VmSpecialty> m_specialty
            = new ObservableCollection<VmSpecialty>();
        SaaMedEntities ctx = new SaaMedEntities();
        public string Fio { get; set; }
        public bool Active { get; set; }

        public ObservableCollection<VmSpecialty> SpecialtyList { get => m_specialty; }
        public ObservableCollection<VmSpecialty> SpecialtyListBox { get; private set; }
            = new ObservableCollection<VmSpecialty>();

        public EditPersonalViewModel()
        {
            FillSpecialty();
        }
        public EditPersonalViewModel(Personal obj)
        {
            Active = obj.Active;
            Fio = obj.Fio;
            FillSpecialty();
            foreach (var o in obj.PersonalSpecialty)
            {
                SpecialtyListBox.Add(new VmSpecialty(o.Specialty));
            }
        }
        private void FillSpecialty()
        {
            lst = ctx.Specialty.ToList();
            foreach (var sp in lst.Where(s => !s.ParentId.HasValue)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                BuildTree(sp);
                m_specialty.Add(sp);
            }
        }
        private void BuildTree(VmSpecialty sp)
        {
            sp.ChildSpecialties.Clear();
            foreach (var sp0 in lst.Where(s => s.ParentId == sp.Id)
                .Select(s => new VmSpecialty(s) { Cargo = SelectedItemMethod }))
            {
                sp0.ParentSpecialty = sp;
                sp.ChildSpecialties.Add(sp0);
                BuildTree(sp0);
            }
        }
        private void SelectedItemMethod(VmSpecialty o)
        {
            SpecialtySel = o;
        }
        private VmSpecialty _selectedItem = null;
        public VmSpecialty SpecialtySel
        {
            get { return _selectedItem; }
            private set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                }
            }
        }
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
            get => new RelayCommand(AddSpecialty, (x) => SpecialtySel != null);
        }
        private void AddSpecialty(object obj)
        {
            if (!SpecialtyListBox.Contains(SpecialtySel, new VmSpecialtyEqualityComparer()))
            {
                SpecialtyListBox.Add(new VmSpecialty(SpecialtySel));
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
            OnPropertyChanged("SpecialtyListBox");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaMedW.ViewModel
{
    public class VmSpecialty : ViewModelBase, IDataErrorInfo
    {
        public VmSpecialty()
        {
        }
        public VmSpecialty(Specialty obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            ParentId = obj.ParentId;
        }
        public VmSpecialty(VmSpecialty obj)
        {
            Id = obj.Id;
            Name = obj.Name;
            ParentId = obj.ParentId;
        }
        private int _Id;
        public int Id
        {
            get => _Id;
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }
        private string _Name;
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged("Name");
            }
        }
        private int? _ParentId;
        public int? ParentId
        {
            get => _ParentId;
            set
            {
                _ParentId = value;
                OnPropertyChanged("ParentId");
            }
        }
        private ICollection<VmSpecialty> _ChildSpecialties = 
            new ObservableCollection<VmSpecialty>();
        public ICollection<VmSpecialty> ChildSpecialties
        {
            get => _ChildSpecialties;
            set
            {
                _ChildSpecialties = value;
                OnPropertyChanged("ChildSpecialties");
            }
        }
        private VmSpecialty _ParentSpecialty;
        public VmSpecialty ParentSpecialty
        {
            get => _ParentSpecialty;
            set
            {
                _ParentSpecialty = value;
                OnPropertyChanged("ParentSpecialty");
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
                        if (String.IsNullOrWhiteSpace(Name))
                            result = "Не введено наименование.";
                        break;
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error => "";

        public delegate void CargoDelegate(VmSpecialty o);
        public CargoDelegate Cargo { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_isSelected)
                    {
                        Cargo(this);
                    }
                }
            }
        }
    }

    public class VmSpecialtyEqualityComparer : IEqualityComparer<VmSpecialty>
    {
        public bool Equals(VmSpecialty b1, VmSpecialty b2)
        {
            return b1.Id == b2.Id;
        }

        public int GetHashCode(VmSpecialty bx)
        {
            return bx.Id;
        }
    }
}

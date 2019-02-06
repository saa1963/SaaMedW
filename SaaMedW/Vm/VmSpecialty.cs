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
            m_Id = obj.Id;
            m_Name = obj.Name;
            m_ParentId = obj.ParentId;
        }
        public VmSpecialty(VmSpecialty obj)
        {
            m_Id = obj.Id;
            m_Name = obj.Name;
            m_ParentId = obj.ParentId;
        }
        private int m_Id;
        public int Id
        {
            get => m_Id;
            set
            {
                m_Id = value;
                OnPropertyChanged("Id");
            }
        }
        private string m_Name;
        public string Name
        {
            get => m_Name;
            set
            {
                m_Name = value;
                OnPropertyChanged("Name");
            }
        }
        private int? m_ParentId;
        public int? ParentId
        {
            get => m_ParentId;
            set
            {
                m_ParentId = value;
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
        public bool ReallyThisBenefit { get; set; } = false;
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

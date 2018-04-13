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
    public class VmPersonal : ViewModelBase, IDataErrorInfo
    {
        private Personal m_object;
        private SaaMedEntities ctx = new SaaMedEntities();
        private ObservableCollection<VmSpecialty> m_specialtylist = new ObservableCollection<VmSpecialty>();

        public VmPersonal()
        {
            m_object = new Personal();
            FillSpecialty();
        }
        public VmPersonal(Personal par)
        {
            m_object = par;
            FillSpecialty();
        }
        public VmPersonal(VmPersonal obj)
        {
            m_object = new Personal();
            Id = obj.Id;
            CopyProperties(obj);
            m_object.Specialty1 = ctx.Specialty.Find(obj.Specialty);
        }
        public VmPersonal Copy(VmPersonal obj)
        {
            CopyProperties(obj);
            return this;
        }
        private void CopyProperties(VmPersonal obj)
        {
            Fio = obj.Fio;
            Specialty = obj.Specialty;
            Active = obj.Active;
        }
        private void FillSpecialty()
        {
            foreach (Specialty o in ctx.Specialty)
            {
                m_specialtylist.Add(new VmSpecialty(o));
            }
        }
        public ObservableCollection<VmSpecialty> SpecialtyList
        {
            get => m_specialtylist;
        }
        public VmSpecialty SpecialtyCurrent
        {
            get => viewSpecialty.CurrentItem as VmSpecialty;
            set { viewSpecialty.MoveCurrentTo(value); }
        }
        private ICollectionView viewSpecialty
        {
            get => CollectionViewSource.GetDefaultView(m_specialtylist);
        }
        public Personal Obj
        {
            get => m_object;
            set
            {
                m_object = value;
                OnPropertyChanged("Obj");
            }
        }
        public int Id
        {
            get => m_object.Id;
            set
            {
                m_object.Id = value;
                OnPropertyChanged("Id");
            }
        }
        public string Fio
        {
            get => m_object.Fio;
            set
            {
                m_object.Fio = value;
                OnPropertyChanged("Fio");
            }
        }
        public int? Specialty
        {
            get => m_object.Specialty;
            set
            {
                m_object.Specialty = value;
                OnPropertyChanged("Specialty");
            }
        }
        public string SpecialtyName
        {
            get => m_object.Specialty1.Name;
        }
        public bool Active
        {
            get => m_object.Active;
            set
            {
                m_object.Active = value;
                OnPropertyChanged("Active");
            }
        }
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
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error => "";
    }
}

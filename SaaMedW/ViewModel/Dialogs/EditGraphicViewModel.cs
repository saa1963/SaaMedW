using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SaaMedW.ViewModel
{
    public class EditGraphicViewModel : ViewModelBase, IDataErrorInfo
    {
        SaaMedEntities ctx = new SaaMedEntities();
        private List<Personal> m_personal;
        private int m_h1, m_m1, m_h2, m_m2;

        public EditGraphicViewModel()
        {
            m_personal = ctx.Personal
                .Where(s => s.Active).OrderBy(s => s.Fio).ToList();
        }
        public List<Personal> SotrList
        {
            get => m_personal;
        }
        public Personal SotrCurrent
        {
            get
            {
                return view.CurrentItem as Personal;
            }
            set
            {
                Personal currentPersonal = null;
                if (value != null)
                    currentPersonal = m_personal.Find(s => s.Id == value.Id);
                var success = view.MoveCurrentTo(currentPersonal);
                OnPropertyChanged("SotrCurrent");
            }
        }
        private ICollectionView view
        {
            get => CollectionViewSource.GetDefaultView(m_personal);
        }
        public int H1
        {
            get => m_h1;
            set
            {
                m_h1 = value;
                OnPropertyChanged("H1");
            }
        }
        public int M1
        {
            get => m_m1;
            set
            {
                m_m1 = value;
                OnPropertyChanged("M1");
            }
        }
        public int H2
        {
            get => m_h2;
            set
            {
                m_h2 = value;
                OnPropertyChanged("H2");
            }
        }
        public int M2
        {
            get => m_m2;
            set
            {
                m_m2 = value;
                OnPropertyChanged("M2");
            }
        }
        public string this[string columnName]
        {
            get
            {
                var result = String.Empty;
                switch (columnName)
                {
                    //case "Fio":
                    //    if (String.IsNullOrWhiteSpace(Fio))
                    //        result = "Не введена ФИО.";
                    //    break;
                    default:
                        break;
                }
                return result;
            }
        }
        public string Error => "";
    }
}

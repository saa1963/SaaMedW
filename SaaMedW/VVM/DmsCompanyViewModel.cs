using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SaaMedW
{
    class DmsCompanyViewModel: NotifyPropertyChanged
    {
        private SaaMedEntities ctx;
        private ObservableCollection<VmDmsCompany> m_DmsCompanyList
            = new ObservableCollection<VmDmsCompany>();
        public ObservableCollection<VmDmsCompany> DmsCompanyList
        {
            get => m_DmsCompanyList;
            set
            {
                m_DmsCompanyList = value;
                OnPropertyChanged("DmsCompanyList");
            }
        }
        public ICollectionView View
        {
            get
            {
                return CollectionViewSource.GetDefaultView(DmsCompanyList);
            }
        }
        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddDmsCompany);
            }
        }
        public DmsCompanyViewModel(SaaMedEntities _ctx)
        {
            ctx = _ctx;
            foreach(var o in ctx.DmsCompany.OrderBy(s => s.Name))
            {
                m_DmsCompanyList.Add(new VmDmsCompany(o));
            }
        }
        private void AddDmsCompany(object obj)
        {
            var modelView = new EditStringViewModel
            {
                Header = "Название ДМС компании"
            };
            var f = new EditStringView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                var p = new DmsCompany
                {
                    Name = modelView.Name
                };
                ctx.DmsCompany.Add(p);
                ctx.SaveChanges();
                var vp = new VmDmsCompany(p);
                DmsCompanyList.Add(vp);
                View.MoveCurrentTo(vp);
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditDmsCompany, s => View.CurrentItem != null);
            }
        }

        private void EditDmsCompany(object obj)
        {
            var dmscompany = View.CurrentItem as VmDmsCompany;
            var modelView = new EditStringViewModel()
            {
                Header = "Название ДМС компании",
                Name = dmscompany.Name
            };
            var f = new EditStringView() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                dmscompany.Name = modelView.Name;
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelDmsCompany, s => View.CurrentItem != null);
            }
        }

        private void DelDmsCompany(object obj)
        {
            try
            {
                var dmscompany = View.CurrentItem as VmDmsCompany;
                ctx.DmsCompany.Remove(dmscompany.Obj);
                ctx.SaveChanges();
                DmsCompanyList.Remove(dmscompany);
            }
            catch (InvalidOperationException e)
            {
                if (e.HResult == -2146233079)
                {
                    MessageBox.Show("Есть ссылки на данную запись. Удаление невозможно");
                }
                else
                {
                    throw e;
                }
            }
        }

        private string m_SearchString;
        public string SearchString
        {
            get => m_SearchString;
            set
            {
                m_SearchString = value;
                FilterList();
                OnPropertyChanged("SearchString");
            }
        }

        private void FilterList()
        {
            View.Filter = NewFilter;
        }

        private bool NewFilter(object obj)
        {
            var s = (VmDmsCompany)obj;
            return s.Name.ToUpper().Contains(m_SearchString.ToUpper());
        }

        private bool? m_CloseDialog;
        public bool? CloseDialog
        {
            get => m_CloseDialog;
            set
            {
                m_CloseDialog = value;
                OnPropertyChanged("CloseDialog");
            }
        }

        public RelayCommand DoubleClickCommand
            => new RelayCommand(SelectRow);

        private void SelectRow(object obj)
        {
            if (obj == null) return;
            CloseDialog = true;
        }
    }
}

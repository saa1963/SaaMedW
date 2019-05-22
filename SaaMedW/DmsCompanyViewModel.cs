using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public VmDmsCompany DmsCompanySel { get; set; }
        private ICollectionView View
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
                return new RelayCommand(EditPersonal);
            }
        }

        private void EditPersonal(object obj)
        {
            //if (PersonalSel == null) return;
            //var personal = PersonalSel as VmPersonal;
            //var modelView = new EditPersonalViewModel(personal.Obj); ;
            //var f = new EditPersonal() { DataContext = modelView };
            //if (f.ShowDialog() ?? false)
            //{
            //    personal.Fio = modelView.Fio;
            //    personal.Active = modelView.Active;

            //    ctx.PersonalSpecialty.RemoveRange(personal.Obj.PersonalSpecialty);
            //    personal.PersonalSpecialty.Clear();

            //    foreach (var o in modelView.SpecialtyListBox)
            //    {
            //        var o1 = ctx.Specialty.Find(o.Id);
            //        personal.PersonalSpecialty.Add(new PersonalSpecialty { Specialty = o1 });
            //    }
            //    ctx.SaveChanges();
            //}
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelPersonal);
            }
        }

        private void DelPersonal(object obj)
        {
            //try
            //{
            //    if (PersonalSel == null) return;
            //    var personal = PersonalSel as VmPersonal;
            //    ctx.Personal.Remove(personal.Obj);
            //    ctx.SaveChanges();
            //    PersonalList.Remove(personal);
            //}
            //catch (InvalidOperationException e)
            //{
            //    if (e.HResult == -2146233079)
            //    {
            //        MessageBox.Show("Есть ссылки на данную запись. Удаление невозможно");
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
        }
    }
}

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
    public class CardsViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<VmPerson> m_cards = new ObservableCollection<VmPerson>();

        public CardsViewModel()
        {
            foreach (var o in ctx.Person)
            {
                m_cards.Add(new VmPerson(o));
            }
        }
        public ObservableCollection<VmPerson> CardsList
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
            //var modelView = new EditUserViewModel();
            //var f = new frmEditUser() { DataContext = modelView };
            //if (f.ShowDialog() ?? false)
            //{
            //    var user = new Users();
            //    user.Fio = modelView.Fio;
            //    user.Login = modelView.Login;
            //    user.Role = modelView.RoleSel.Id;
            //    if (!String.IsNullOrWhiteSpace(modelView.Password))
            //    {
            //        user.Password = new System.Security.Cryptography.SHA1CryptoServiceProvider()
            //            .ComputeHash(System.Text.Encoding.ASCII.GetBytes(modelView.Password));
            //    }
            //    user.Disabled = modelView.Disabled;
            //    ctx.Users.Add(user);
            //    ctx.SaveChanges();
            //    var vmuser = new VmUsers(user);
            //    UsersList.Add(vmuser);
            //    viewUsers.MoveCurrentTo(vmuser);
            //}
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
            //if (UsersSel == null) return;
            //var user = UsersSel as VmUsers;
            //var modelView = new EditUserViewModel();
            //modelView.Fio = user.Fio;
            //modelView.Login = user.Login;
            //modelView.RoleSel = modelView.Roles.FirstOrDefault(s => s.Id == user.Role);
            //modelView.IsEnablePassword = false;
            //modelView.Disabled = user.Disabled;
            //var f = new frmEditUser() { DataContext = modelView };
            //if (f.ShowDialog() ?? false)
            //{
            //    user.Fio = modelView.Fio;
            //    user.Login = modelView.Login;
            //    user.Role = modelView.RoleSel.Id;
            //    user.Disabled = modelView.Disabled;
            //    ctx.SaveChanges();
            //}
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
            //if (UsersSel == null) return;
            //var user = UsersSel as VmUsers;
            //ctx.Users.Remove(user.users);
            //ctx.SaveChanges();
            //UsersList.Remove(user);
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
            var person = CardsSel as VmPerson;
            new MedCard().DoIt(person.Obj);
        }
    }
}

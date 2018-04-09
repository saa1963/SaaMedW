using SaaMedW.View;
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
    public class UsersViewModel : ViewModelBase
    {
        private SaaMedEntities ctx = new SaaMedEntities();
        private readonly ObservableCollection<VmUsers> m_users = new ObservableCollection<VmUsers>();

        public UsersViewModel()
        {
            foreach (var o in  ctx.Users)
            {
                m_users.Add(new VmUsers(o));
            }
        }
        public ObservableCollection<VmUsers> UsersList
        {
            get { return m_users; }
        }
        public object UsersSel
        {
            get { return viewUsers.CurrentItem; }
            set { viewUsers.MoveCurrentTo(value); }
        }
        private ICollectionView viewUsers
        {
            get
            {
                return CollectionViewSource.GetDefaultView(UsersList);
            }
        }
        public RelayCommand Add
        {
            get
            {
                return new RelayCommand(AddUser);
            }
        }

        private void AddUser(object obj)
        {
            var modelView = new EditUserViewModel();
            var f = new frmEditUser() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                var user = new Users();
                user.Fio = modelView.Fio;
                user.Login = modelView.Login;
                user.Role = modelView.RoleSel.Id;
                if (!String.IsNullOrWhiteSpace(modelView.Password))
                {
                    user.Password = new System.Security.Cryptography.SHA1CryptoServiceProvider()
                        .ComputeHash(System.Text.Encoding.ASCII.GetBytes(modelView.Password));
                }
                user.Disabled = modelView.Disabled;
                ctx.Users.Add(user);
                ctx.SaveChanges();
                var vmuser = new VmUsers(user);
                UsersList.Add(vmuser);
                viewUsers.MoveCurrentTo(vmuser);
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditUser);
            }
        }

        private void EditUser(object obj)
        {
            if (UsersSel == null) return;
            var user = UsersSel as VmUsers;
            var modelView = new EditUserViewModel();
            modelView.Fio = user.Fio;
            modelView.Login = user.Login;
            modelView.RoleSel = modelView.Roles.FirstOrDefault(s => s.Id == user.Role);
            modelView.IsEnablePassword = false;
            modelView.Disabled = user.Disabled;
            var f = new frmEditUser() { DataContext = modelView };
            if (f.ShowDialog() ?? false)
            {
                user.Fio = modelView.Fio;
                user.Login = modelView.Login;
                user.Role = modelView.RoleSel.Id;
                user.Disabled = modelView.Disabled;
                ctx.SaveChanges();
            }
        }

        public RelayCommand Del
        {
            get
            {
                return new RelayCommand(DelUser);
            }
        }

        private void DelUser(object obj)
        {
            throw new NotImplementedException();
        }

        public RelayCommand ChangePassword
        {
            get
            {
                return new RelayCommand(changePassword);
            }
        }

        private void changePassword(object obj)
        {
            throw new NotImplementedException();
        }
    }
}

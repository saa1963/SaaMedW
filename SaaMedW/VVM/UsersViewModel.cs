using log4net;
using SaaMedW.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SaaMedW
{
    public class UsersViewModel
    {
        ILog log;
        private SaaMedEntities ctx = new SaaMedEntities();

        public UsersViewModel()
        {
            log = log4net.LogManager.GetLogger(this.GetType());
            foreach (var o in  ctx.Users)
            {
                UsersList.Add(new VmUsers(o));
            }
        }
        public ObservableCollection<VmUsers> UsersList { get; set; }
            = new ObservableCollection<VmUsers>();

        public VmUsers UsersSel { get; set; }

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
            try
            {
                var modelView = new EditUserViewModel();
                var f = new EditUserView() { DataContext = modelView };
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
            catch(DbEntityValidationException e)
            {
                var msg = "Ошибка добавления пользователя";
                log.Error(msg, e);
            }
        }

        public RelayCommand Edit
        {
            get
            {
                return new RelayCommand(EditUser, s => UsersSel != null);
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
            var f = new EditUserView() { DataContext = modelView };
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
                return new RelayCommand(DelUser, s => UsersSel != null);
            }
        }

        private void DelUser(object obj)
        {
            if (UsersSel == null) return;
            var user = UsersSel as VmUsers;
            if (user.Login != "Service")
            {
                ctx.Users.Remove(user.users);
                ctx.SaveChanges();
                UsersList.Remove(user);
            }
            else
            {
                MessageBox.Show("Нельзя удалять встроенного пользователя.");
            }
        }

        public RelayCommand ChangePassword
        {
            get
            {
                return new RelayCommand(changePassword, s => UsersSel != null);
            }
        }

        private void changePassword(object obj)
        {
            if (UsersSel == null) return;
            var user = (UsersSel as VmUsers).users;

            var mv = new ChangePasswordViewModel();
            var f = new ChangePassword { DataContext = mv };
            var r = f.ShowDialog();
            if (r ?? false)
            {
                if (!String.IsNullOrWhiteSpace(mv.NewPassword))
                {
                    user.Password = new System.Security.Cryptography.SHA1CryptoServiceProvider()
                            .ComputeHash(System.Text.Encoding.ASCII.GetBytes(mv.NewPassword));
                    ctx.SaveChanges();
                    System.Windows.MessageBox.Show("Пароль изменен.");
                }
                else
                {
                    user.Password = null;
                    ctx.SaveChanges();
                    System.Windows.MessageBox.Show("Пароль сброшен.");
                }
            }
        }
    }
}

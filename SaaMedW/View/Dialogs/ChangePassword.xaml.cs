using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SaaMedW.View
{
    /// <summary>
    /// Логика взаимодействия для ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidationDialog.IsValid(this))
            {
                MessageBox.Show("Не введены данные.");
                return;
            }
            if (tbNewPassword0.Text != tbRepPassword0.Text)
            {
                MessageBox.Show("Пароли не совпадают.");
                return;
            }
            DialogResult = true;
        }

        private void tbRepPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            tbRepPassword0.Text = tbRepPassword.Password;
        }

        private void tbNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            tbNewPassword0.Text = tbNewPassword.Password;
        }
    }
}

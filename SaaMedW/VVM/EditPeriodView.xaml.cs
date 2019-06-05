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

namespace SaaMedW
{
    /// <summary>
    /// Логика взаимодействия для EditPeriodView.xaml
    /// </summary>
    public partial class EditPeriodView : Window
    {
        public EditPeriodView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidationDialog.IsValid(this))
            {
                return;
            }
            DialogResult = true;
        }
    }
}

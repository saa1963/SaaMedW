using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для SelectSpecialtyView.xaml
    /// </summary>
    public partial class SelectSpecialtyView : Window
    {
        public SelectSpecialtyView()
        {
            InitializeComponent();
        }

        private void MouseDoubleClickHandler(object sender, MouseButtonEventArgs e)
        {
            if (!(DialogResult ?? false))
                DialogResult = true;
        }
    }
}

using SaaMedW.ViewModel;
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
    /// Логика взаимодействия для EditVisitView.xaml
    /// </summary>
    public partial class EditVisitView : UserControl
    {
        public EditVisitView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void BenefitButton_Click(object sender, RoutedEventArgs e)
        {
            tbPopup.IsOpen = !tbPopup.IsOpen;
        }


    }
}

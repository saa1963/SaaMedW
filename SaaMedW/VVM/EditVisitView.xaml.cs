using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SaaMedW
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

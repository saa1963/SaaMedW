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

        public void CanExecuteButton(object sender, CanExecuteRoutedEventArgs e )
        {
            if (PersonCombo.SelectedValue != null)
            {
                e.CanExecute = true;
            }
        }

        public void ExecuteButton(object sender, ExecutedRoutedEventArgs e)
        {
            var viewmodel = this.DataContext as EditVisitViewModel;
            viewmodel.AddVisit(e.Parameter as VisitTimeInterval);
        }
    }
}

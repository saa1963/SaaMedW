using SaaMedW.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaaMedW.View
{
    /// <summary>
    /// Логика взаимодействия для GraphicView.xaml
    /// </summary>
    public partial class GraphicView : UserControl
    {
        private List<Border> borderList = new List<Border>();
        public GraphicView()
        {
            InitializeComponent();
        }

        private void Init()
        {
            int row = 1, col = 0, ind = 0;
            while (row < 7)
            {
                col = 0;
                while (col < 7)
                {
                    var uc = new GraphicControlView();
                    
                    uc.SetBinding(UserControl.DataContextProperty, $"Mas[{ind}]");
                    uc.addSotr.CommandParameter = ind;
                    
                    Grid.SetColumn(uc, col);
                    Grid.SetRow(uc, row);
                    g1.Children.Add(uc);
                    col++;
                    ind++;
                }
                row++;
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Init();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}

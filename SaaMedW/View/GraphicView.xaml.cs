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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaaMedW.View
{
    /// <summary>
    /// Логика взаимодействия для GraphicView.xaml
    /// </summary>
    public partial class GraphicView : UserControl
    {
        public GraphicView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int row = 1, col = 0;
            while (row <= 6)
            {
                col = 0;
                while (col <= 6)
                {
                    var b1 = new Border();
                    b1.BorderBrush = Brushes.Black;
                    b1.BorderThickness = new Thickness(1);
                    g1.Children.Add(b1);
                    Grid.SetColumn(b1, col);
                    Grid.SetRow(b1, row);
                    var scrollViewer = new ScrollViewer();
                    scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    b1.Child = scrollViewer;

                    var sp = new StackPanel();
                    scrollViewer.Content = sp;
                    var tb0 = new TextBlock();

                    var r1 = new Run();
                    r1.FontWeight = FontWeights.Bold;
                    var converter = new System.Windows.Media.BrushConverter();
                    var brush = (Brush)converter.ConvertFromString("#FF1B1180");
                    r1.Foreground = brush;
                    var bind1 = new Binding("Mas[0].Dt");
                    bind1.StringFormat = "dd.MM.yy";
                    r1.SetBinding(Run.TextProperty, bind1);
                    tb0.Inlines.Add(r1);
                    sp.Children.Add(tb0);

                    var b2 = new Border();
                    b2.CornerRadius = new CornerRadius(6);
                    b2.SetResourceReference(
                        Control.BackgroundProperty,
                        SystemColors.GradientActiveCaptionBrushKey);
                    sp.Children.Add(b2);
                    var tb = new TextBlock();
                    tb.Margin = new Thickness(5);
                    tb.TextWrapping = TextWrapping.Wrap;
                    b2.Child = tb;
                    
                    var r2 = new Run();
                    r2.SetBinding(Run.TextProperty, "Mas[0].personal.Fio");
                    tb.Inlines.Add(r2);
                    tb.Inlines.Add(" ");
                    var r3 = new Run();
                    r3.SetBinding(Run.TextProperty, "Mas[0].personal.Specialty1.Name");
                    tb.Inlines.Add(r3);
                    tb.Inlines.Add(" ");
                    var r4 = new Run();
                    var bind2 = new Binding("Mas[0].Interval");
                    bind2.Mode = BindingMode.OneWay;
                    r4.SetBinding(Run.TextProperty, bind2);
                    tb.Inlines.Add(r4);
                    col++;
                }
                row++;
            }
        }
    }
}

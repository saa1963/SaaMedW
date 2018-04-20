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

        public void Init()
        {
            int row = 1, col = 0, ind = 0;

            List<VmGraphic> lst;
            var dataContext = this.DataContext as GraphicViewModel;
            DateTime dt = dataContext.Dt1;

            foreach (var o in borderList)
            {
                g1.Children.Remove(o);
            }

            while (row <= 6)
            {
                col = 0;
                while (col <= 6)
                {
                    if (dataContext.PersonalCurrent == null)
                        lst = dataContext.Mas[ind];
                    else
                        lst = dataContext.Mas[ind].Where(s => s.PersonalId == dataContext.PersonalCurrent.Id).ToList();
                    var b1 = new Border();
                    b1.BorderBrush = Brushes.Black;
                    b1.BorderThickness = new Thickness(1);
                    b1.Background = Brushes.Transparent;
                    g1.Children.Add(b1);
                    borderList.Add(b1);
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
                    var bind1 = new Binding($"Dt[{ind}]");
                    bind1.StringFormat = "dd.MM.yy";
                    r1.SetBinding(Run.TextProperty, bind1);
                    tb0.Inlines.Add(r1);
                    sp.Children.Add(tb0);

                    var bind00 = new Binding("AddSotr");
                    var mi1 = new MenuItem();
                    mi1.Header = "Добавить сотрудника";
                    mi1.SetBinding(MenuItem.CommandProperty, bind00);
                    mi1.CommandParameter = ind;
                    var menu = new ContextMenu();
                    menu.Items.Add(mi1);
                    b1.ContextMenu = menu;

                    for (var j = 0; j < lst.Count; j++)
                    {
                        var b2 = new Border();
                        b2.CornerRadius = new CornerRadius(6);
                        b2.SetResourceReference(
                            Control.BackgroundProperty,
                            SystemColors.GradientActiveCaptionBrushKey);

                        var tb = new TextBlock();
                        tb.Margin = new Thickness(5);
                        tb.TextWrapping = TextWrapping.Wrap;

                        var r2 = new Run();
                        r2.SetBinding(Run.TextProperty, $"Mas[{ind}][{j}].personal.Fio");

                        var r3 = new Run();
                        r3.SetBinding(Run.TextProperty, $"Mas[{ind}][{j}].personal.Specialty1.Name");

                        var bind2 = new Binding($"Mas[{ind}][{j}].Interval");
                        bind2.Mode = BindingMode.OneWay;

                        var r4 = new Run();
                        r4.SetBinding(Run.TextProperty, bind2);

                        sp.Children.Add(b2);
                        b2.Child = tb;
                        tb.Inlines.Add(r2);
                        tb.Inlines.Add(" ");
                        tb.Inlines.Add(r3);
                        tb.Inlines.Add(" ");
                        tb.Inlines.Add(r4);
                    }
                    col++;
                    ind++;
                    dt = dt.AddDays(1);
                }
                row++;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }
    }
}

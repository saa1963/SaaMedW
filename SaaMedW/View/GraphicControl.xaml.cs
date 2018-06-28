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
    /// Логика взаимодействия для GraphicControl.xaml
    /// </summary>
    public partial class GraphicControl : UserControl
    {
        public GraphicControl()
        {
            InitializeComponent();
        }

        private void GraphicControl_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(typeof(VmGraphic)) as VmGraphic;
            var ob = DataContext as ListGraphicViewModel;
            var ti2 = new TimeInterval(data.Dt, data.H1, data.M1, data.H2, data.M2);
            bool intersected = false;
            foreach (var o in ob)
            {
                if (o.PersonalId == data.PersonalId)
                {
                    var ti1 = new TimeInterval(data.Dt, o.H1, o.M1, o.H2, o.M2);
                    if (ti1.IsIntersected(ti2))
                    {
                        intersected = true;
                        break;
                    }
                }
            }
            if (ob.Dt.Date >= DateTime.Today && !intersected)
            {
                ob.AddGraphic(data);
            }
        }

        private void TextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            ListGraphicViewModel o;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                o = DataContext as ListGraphicViewModel;
                if (o.Current != null)
                {
                    DataObject data = new DataObject(o.Current);
                    DragDrop.DoDragDrop(sender as TextBlock, data, DragDropEffects.Copy);
                }
            }
        }
    }
}

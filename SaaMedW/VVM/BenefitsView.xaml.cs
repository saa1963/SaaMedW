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

namespace SaaMedW
{
    /// <summary>
    /// Логика взаимодействия для BenefitsView.xaml
    /// </summary>
    public partial class BenefitsView : UserControl
    {
        public BenefitsView()
        {
            InitializeComponent();
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var lst = new List<VmBenefit>();
                foreach(var o in g.SelectedItems)
                {
                    System.Diagnostics.Debug.WriteLine(o.GetHashCode());
                    lst.Add(o as VmBenefit);
                }
                DataObject data = new DataObject(lst);
                DragDrop.DoDragDrop(sender as DataGridCell, data, DragDropEffects.Move);
            }
        }

        private void DropHandler(object sender, DragEventArgs e)
        {
            var mv = DataContext as BenefitsViewModel;
            var targetSpecialty = (sender as TreeViewItem).DataContext as VmSpecialty;
            var sourceBenefitsList = e.Data.GetData(typeof(List<VmBenefit>)) as List<VmBenefit>;
            mv.Move2OtherSpecialty(sourceBenefitsList, targetSpecialty);
            e.Handled = true;
        }
    }
}

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
    /// Логика взаимодействия для SpecialtyView.xaml
    /// </summary>
    public partial class SpecialtyView : UserControl
    {
        public SpecialtyView()
        {
            InitializeComponent();
        }

        private void DropHandler(object sender, DragEventArgs e)
        {
            var mv = DataContext as SpecialtyViewModel;
            var ctx = (DataContext as SpecialtyViewModel).ctx;
            var source = e.Data.GetData(typeof(VmSpecialty)) as VmSpecialty;
            var target = (sender as TreeViewItem).DataContext as VmSpecialty;
            if (source.ParentSpecialty != null)
            {
                source.ParentSpecialty.ChildSpecialties.Remove(source);
            }
            source.ParentId = target.Id;
            source.ParentSpecialty = target;
            target.ChildSpecialties.Add(new VmSpecialty(ctx.Specialty.Find(source.Id)) { Cargo = mv.SelectedItemMethod });
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var o = (sender as TreeViewItem).DataContext as VmSpecialty;
                DataObject data = new DataObject(o);
                DragDrop.DoDragDrop(sender as TreeViewItem, data, DragDropEffects.Copy);
            }
        }
    }
}

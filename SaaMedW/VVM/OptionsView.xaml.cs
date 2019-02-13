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
    /// Логика взаимодействия для OptionsView.xaml
    /// </summary>
    public partial class OptionsView : UserControl
    {
        public OptionsView()
        {
            InitializeComponent();
        }

        private void ParameterChanged(object sender, TextChangedEventArgs e)
        {
            SetIsChanged(sender);
        }

        private void DateTimeParameterChanged(object sender, SelectionChangedEventArgs e)
        {
            SetIsChanged(sender);
        }

        private void SetIsChanged(object sender)
        {
            var viewModel = DataContext as ViewModel.OptionsViewModel;
            viewModel.IsChanged = true;
        }
    }
}
